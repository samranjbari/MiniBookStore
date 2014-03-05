using System;
using System.Net.Mail;
using Books.Data.API;
using Books.Data.Models;
using ServiceStack.Configuration;

namespace Books.Data.Service
{
    public class EmailService : IEmailService
    {
        public void SendSmtpEmail(EmailServiceModels model)
        {
            var appSetting = new AppSettings();
            MailMessage mail = new MailMessage();

            mail.Body = model.Body;
            mail.IsBodyHtml = model.IsBodyHtml;
            mail.Priority = MailPriority.High;
            mail.Subject = model.Subject;

            foreach (var item in model.Attachments)
            {
                mail.Attachments.Add(new Attachment(item));
            }

            foreach (var to in model.To)
            {
                mail.To.Add(to);
            }

            new SmtpClient().SendAsync(mail, new Guid().ToString());
        }
    }
}
