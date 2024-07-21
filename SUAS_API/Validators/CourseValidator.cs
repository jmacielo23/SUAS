using FluentValidation;
using SUAS_API.Models;

namespace SUAS_API.Validators
{
    public class CourseValidator: AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(x=>x.Code)
                .NotEmpty()
                .Length(5,10).WithMessage("Code must be between 5 and 10 characters long.")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Code must be alphanumeric (letters and numbers only) and cannot contain special characters.");
            RuleFor(x=>x.Title)
                .NotEmpty()
                .Length(5, 50).WithMessage("Title must be between 5 and 50 characters long.")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Code must be alphanumeric (letters and numbers only) and cannot contain special characters.");
            RuleFor(x => x.Credits).GreaterThan(0);
        }
    }
}
