using FluentValidation;

namespace HrApp.Application.Calendars.Command.CreateCalendarEvent;

public class CreateCalendarEventCommandValidator : AbstractValidator<CreateCalendarEventCommand>
{
    public CreateCalendarEventCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(60).WithMessage("Title maximum length is 60 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.")
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Start date must be in the future.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after the start date.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200)
            .WithMessage("Description maximum length is 200 characters.");
    }
}