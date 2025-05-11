using MediatR;

namespace HrApp.Application.Authorizations.Command.Validate2FA;

public class Validate2FARequest : IRequest<string>
{
    public int Code { get; set; }
}
