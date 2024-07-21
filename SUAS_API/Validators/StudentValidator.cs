using FluentValidation;
using SUAS_API.Models;

namespace SUAS_API.Validators
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Matches(@"^[^\d]*(?:[^\w\s]|[a-zA-Z])+$").WithMessage("First Name cannot contain numeric characters")
                .Length(3, 20);
            RuleFor(x => x.LastName)
                .NotEmpty()
                .Matches(@"^[^\d]*(?:[^\w\s]|[a-zA-Z])+$").WithMessage("Last Name cannot contain numeric characters")
                .Length(3, 20);
            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past.");
            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in a valid international format.")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
        }
    }
}
