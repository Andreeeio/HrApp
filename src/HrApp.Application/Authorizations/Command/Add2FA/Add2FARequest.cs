using MediatR;

namespace HrApp.Application.Authorizations.Command.Add2FA;

public class Add2FARequest : IRequest
{
    public string Email { get; set; } = default!;
}
