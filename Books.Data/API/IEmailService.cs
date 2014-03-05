using Books.Data.Models;

namespace Books.Data.API
{
    public interface IEmailService
    {
        void SendSmtpEmail(EmailServiceModels model);
    }
}
