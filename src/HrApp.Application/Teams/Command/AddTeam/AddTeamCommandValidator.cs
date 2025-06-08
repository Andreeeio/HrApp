using FluentValidation;

namespace HrApp.Application.Teams.Command.AddTeam;

public class AddTeamCommandValidator : AbstractValidator<AddTeamCommand>
{
    public AddTeamCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required.")
            .MaximumLength(100).WithMessage("Team name cannot exceed 100 characters.");
        
        RuleFor(x => x.TeamLeaderEmail)
            .NotEmpty().WithMessage("Team leader email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
