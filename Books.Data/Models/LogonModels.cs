using ServiceStack.ServiceHost;

namespace Books.Data.Models
{
    [Route("/logon")]
    public class LogonModels : ModelBase
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public bool LogOut { get; set; }

        public bool GetInfo { get; set; }

        public bool ResetPassword { get; set; }

        public string Email { get; set; }
    }
}