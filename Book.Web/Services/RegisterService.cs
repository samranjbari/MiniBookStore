using System;
using System.Collections.Generic;
using Books.Data.Models;
using ServiceStack.FluentValidation.Results;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Text;

namespace Books.Services
{
    public class RegisterService : Service
    {
        public IUserAuthRepository UserAuthRepository { get; set; }
                
        public object Post(AccountModel request)
        {
            var session = this.GetSession();
            ValidationResult validations = new AccountValidator().Validate(request);
            if (!validations.IsValid)
            {
                request.ResponseResult.ResultStatus = ResultStatuses.Error;
                foreach (var item in validations.Errors)
                {
                    request.ResponseResult.Messages.Add(item.ErrorMessage);
                }

                return request;
            }

            string hash, salt;
            new SaltedHash().GetHashAndSaltString(request.Password, out hash, out salt);

            try
            {
                var roles = new List<string>();
                if (request.IsMember)
                {
                    roles.Add("Member");
                }
                else
                {
                    roles.Add("Author");
                }

                var newUser = UserAuthRepository.CreateUserAuth(new UserAuth
                {
                    DisplayName = request.UserName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PrimaryEmail = request.Email,
                    UserName = request.UserName,
                    Salt = salt,
                    PasswordHash = hash,
                    FullName = "{0} {1}".Fmt(request.FirstName, request.LastName),
                    Roles = roles
                }, request.Password);
            }
            catch (ArgumentException ex)
            {
                request.ResponseResult.ResultStatus = ResultStatuses.Error;
                request.ResponseResult.Messages.Add(ex.Message);

                return request;
            }

            request.ResponseResult.ResultStatus = ResultStatuses.Success;
            return request;
        }

        public object Get(AccountModel request)
        {
            var userAuth = UserAuthRepository.GetUserAuthByUserName(request.UserName);

            return new
            {
                Id = userAuth.Id,
                FirstName = userAuth.FirstName,
                LastName = userAuth.LastName,
                UserName = userAuth.UserName,
                DisplayName = userAuth.DisplayName,
                Email = userAuth.Email,
                IsMember = userAuth.Roles.Contains("Member")
            };
        }

        [Authenticate]
        public object Put(AccountModel todo)
        {
            return "success";
        }
    }
}