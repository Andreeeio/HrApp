using FluentValidation;

namespace HrApp.Application.Teams.Command.AddEmployer;

public class AddEmployerCommandValidator : AbstractValidator<AddEmployerCommand>
{
    public AddEmployerCommandValidator()
    {
        RuleFor(x => x.UserEmail)
            .NotEmpty()
            .WithMessage("User email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");
    }
}
