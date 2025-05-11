using MediatR;

namespace HrApp.Application.Users.Query.LoginUser;

public class LoginUserQuery : IRequest<(string, int)>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
