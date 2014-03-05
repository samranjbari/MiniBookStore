using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Security;
using Books.Data.API;
using Books.Data.Models;
using Books.ViewModels;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation.Results;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Text;

namespace Books.Services
{
    public class LogonService : Service
    {
        public IUserAuthRepository UserAuthRepository { get; set; }
        public IEmailService EmailService { get; set; }

        public object Any(LogonModels request)
        {
            if (request.GetInfo)
            {
                return this.GetSession().IsAuthenticated;
            }

            if (request.LogOut)
            {
                FormsAuthentication.SignOut();
            }

            if (request.ResetPassword)
            {
                var userAuth = UserAuthRepository.GetUserAuthByUserName(request.Email);

                if (userAuth == null)
                {
                    request.ResponseResult.ResultStatus = ResultStatuses.Warning;
                    request.ResponseResult.Messages.Add("The specified Email address was not found.");
                    return request;
                }

                EmailService.SendSmtpEmail(this.BuildEmailMessage(request.Email));

                request.ResponseResult.ResultStatus = ResultStatuses.Success;
                request.ResponseResult.Messages.Add("Please follow the link sent to your Email to reset your password.");
                return request;
            }

            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return false;
            }

            FormsAuthentication.SetAuthCookie(request.UserName, request.RememberMe);
            return true;
        }

        public object Any(ResetViewModel request)
        {
            ValidationResult validations = new ResetAccountValidator().Validate(request);

            if (!validations.IsValid)
            {
                request.ResponseResult.ResultStatus = ResultStatuses.Error;
                foreach (var item in validations.Errors)
                {
                    request.ResponseResult.Messages.Add(item.ErrorMessage);
                }

                return request;
            }

            string de = request.Hash.Decrypt("hash");

            var userAuth = UserAuthRepository.GetUserAuthByUserName(de);
            if (userAuth != null)
            {
                UserAuthRepository.UpdateUserAuth(userAuth, userAuth, request.Password);
            }

            request.ResponseResult.ResultStatus = ResultStatuses.Success;
            request.ResponseResult.Messages.Clear();

            return request;
        }

        public object Any(UserModels request)
        {
            if (!this.GetSession().IsAuthenticated)
            {
                return null;
            }

            var session = this.GetSession();
            var roles = new List<UserRoles>();

            foreach (var r in session.Roles)
            {
                roles.Add((UserRoles)Enum.Parse(typeof(UserRoles), r));
            }

            return new UserModels
            {
                FirstName = session.FirstName,
                LastName = session.LastName,
                Id = session.Id.To<int>(),
                Username = session.UserName,
                Role = roles
            };
        }

        private EmailServiceModels BuildEmailMessage(string email)
        {
            var appSettings = new AppSettings();
            var mail = new EmailServiceModels();

            mail.IsBodyHtml = true;
            mail.Subject = "Cocaine-books Password Recovery";

            var data = "{0}".Fmt(email);// , DateTime.Now.AddHours(2).ToString("hh-mm-ss"));

            string encrypted = data.Encrypt("hash");

            mail.Body = @"Hello, <br/><br/>

                        Please use the link below to reset your password:<br/>
                        {0}<br/><br/>

                        Thank you,<br/>
                        Cocaine Books".Fmt(appSettings.Get("Site.ResetUrl", "") + "?" + encrypted);

            mail.To.Add(email);

            return mail;
        }
    }
}