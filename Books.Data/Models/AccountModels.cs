using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;

namespace Books.Data.Models
{
    [Route("/account")]
    public class AccountModel : ModelBase, IReturn<AccountModel>
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool AcceptAgreement { get; set; }

        public bool IsMember { get; set; }
    }

    public class AccountValidator : AbstractValidator<AccountModel>
    {
        public AccountValidator()
        {
            RuleFor(e => e.UserName).NotEmpty();
            RuleFor(e => e.Email).NotEmpty();
            RuleFor(e => e.Password).NotEmpty();
            RuleFor(e => e.FirstName).NotEmpty();
            RuleFor(e => e.LastName).NotEmpty();
            RuleFor(e => e.Email).EmailAddress();
            RuleFor(e => e.AcceptAgreement).Equal(true).Unless(a => a.IsMember).WithMessage("Please accept the Terms of Agreement.");
        }
    }
}
