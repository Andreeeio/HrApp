using HrApp.Application.GoogleOAuthTokens.Query.DTO;
using MediatR;

namespace HrApp.Application.GoogleOAuthTokens.Query.ExchangeCodeForToken;

public class ExchangeCodeForTokenQuery : IRequest
{
    public string Code { get; set; } = default!;
}
