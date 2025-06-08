using FluentValidation;

namespace HrApp.Application.Users.Command.FirstLoginUser;

public class FirstLoginUserCommandValidator : AbstractValidator<FirstLoginUserCommand>
{
    public FirstLoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(70).WithMessage("Email cannot exceed 70 characters.");

        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password is required.")
            .MinimumLength(8).WithMessage("Old password must be at least 8 characters long.")
            .MaximumLength(64).WithMessage("Old password cannot exceed 64 characters.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
            .MaximumLength(64).WithMessage("New password cannot exceed 64 characters.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required.")
            .MinimumLength(8).WithMessage("Confirm password must be at least 8 characters long.")
            .MaximumLength(64).WithMessage("Confirm password cannot exceed 64 characters.");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required.")
            .MaximumLength(128).WithMessage("Token cannot exceed 128 characters.");
    }
}
