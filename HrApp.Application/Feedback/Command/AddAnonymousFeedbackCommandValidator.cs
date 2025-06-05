using FluentValidation;

namespace HrApp.Application.Feedback.Command;

public class AddAnonymousFeedbackCommandValidator : AbstractValidator<AddAnonymousFeedbackCommand>
{
    public AddAnonymousFeedbackCommandValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Feedback subject is required.")
            .MaximumLength(200).WithMessage("Feedback subject cannot exceed 200 characters.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Feedback message is required.")
            .MaximumLength(2000).WithMessage("Feedback message cannot exceed 2000 characters.");
    }
}
