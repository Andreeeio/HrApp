using FluentValidation;

namespace HrApp.Application.Department.Command.AddDepartment;

public class AddDepartmentCommandValidator : AbstractValidator<AddDepartmentCommand>
{
    public AddDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required.")
            .MaximumLength(50).WithMessage("Department name cannot exceed 100 characters.");

        RuleFor(x => x.TeamTag)
            .NotEmpty().WithMessage("Team tag is required.")
            .MaximumLength(50).WithMessage("Team tag cannot exceed 50 characters.");

        RuleFor(x => x.HeadOfDepartmentEmail)
            .NotEmpty().WithMessage("Head of department email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
