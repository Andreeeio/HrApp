using FluentValidation;

namespace HrApp.Application.EmploymentHistories.Command.AddEmploymentHistory;

public class AddEmploymentHistoryCommandValidator : AbstractValidator<AddEmploymentHistoryCommand>
{
    public AddEmploymentHistoryCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.")
            .Must(date => date.ToDateTime(TimeOnly.MinValue) > DateTime.UtcNow)
            .WithMessage("Start date must be in the future.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after the start date.");

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("Position is required.")
            .MaximumLength(30).WithMessage("Position cannot exceed 30 characters.");
    }
}
