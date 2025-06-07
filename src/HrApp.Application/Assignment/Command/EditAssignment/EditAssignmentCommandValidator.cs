using FluentValidation;

namespace HrApp.Application.Assignment.Command.EditAssignment;

public class EditAssignmentCommandValidator : AbstractValidator<EditAssignmentCommand>
{
    public EditAssignmentCommandValidator()
    {
        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.");

        RuleFor(x => x.DifficultyLevel)
            .GreaterThan(0).WithMessage("Difficulty level must be greater than 0.")
            .LessThan(6).WithMessage("Difficulty level must be less than 6.");
    }
}
