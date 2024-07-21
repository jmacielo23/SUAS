using FluentValidation;
using SUAS_API.Models;

namespace SUAS_API.Validators
{
    public class ApplcationValidator: AbstractValidator<Application>
    {
        public ApplcationValidator()
        {
            RuleFor(x => x.StudentID)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x=>x.CourseID)
                 .NotEmpty()
                .GreaterThan(0);
            RuleFor(x=>x.ApplicationDate)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Application Date cannot be a future date.")
                .GreaterThanOrEqualTo(DateTime.Now.AddMonths(-3)).WithMessage("Application Date must be within the last 3 months."); ;
        }
    }
}
