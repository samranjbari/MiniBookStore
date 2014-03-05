using Books.Data.Models;
using ServiceStack.ServiceHost;
using ServiceStack.FluentValidation;

namespace Books.ViewModels
{
    [Route("/resetAccount")]
    public class ResetViewModel : ModelBase
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Hash { get; set; }
    }

    public class ResetAccountValidator : AbstractValidator<ResetViewModel>
    {
        public ResetAccountValidator()
        {
            RuleFor(e => e.Password).NotEmpty();
            RuleFor(e => e.ConfirmPassword).NotEmpty();
            RuleFor(e => e.Password).Equal(a => a.ConfirmPassword).WithMessage("Passwords are not matching.");
        }
    }
}