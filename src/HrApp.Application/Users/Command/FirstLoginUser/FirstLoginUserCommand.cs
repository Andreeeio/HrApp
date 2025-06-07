using MediatR;

namespace HrApp.Application.Users.Command.FirstLoginUser;

public class FirstLoginUserCommand : IRequest<string>
{
    public string Email { get; set; } = string.Empty;
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
