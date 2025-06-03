using FluentValidation;

namespace HrApp.Application.Assignment.Command.AddAssignment;

public class AddAssignmentCommandValidator : AbstractValidator<AddAssignmentCommand>
{
    public AddAssignmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Task name is required.")
            .MinimumLength(3).WithMessage("Task name must be at least 3 characters long.")
            .MaximumLength(60).WithMessage("Task name must not exceed 60 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Task description is required.")
            .MaximumLength(300).WithMessage("Task description must not exceed 300 characters.");

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.Now).WithMessage("Task start date must be in the future.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("Task end date must be later than the start date.")
            .GreaterThan(DateTime.Now).WithMessage("Task end date must be in the future.");

        RuleFor(x => x.DifficultyLevel)
            .GreaterThan(0).WithMessage("Difficulty level must be greater than 0.")
            .LessThan(6).WithMessage("Difficulty level must be less than 6.");
    }
}

