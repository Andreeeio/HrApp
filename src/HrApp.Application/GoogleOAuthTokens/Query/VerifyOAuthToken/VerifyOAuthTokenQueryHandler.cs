using HrApp.Application.Interfaces;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.GoogleOAuthTokens.Query.VerifyOAuthToken;

public class VerifyOAuthTokenQueryHandler : IRequestHandler<VerifyOAuthTokenQuery, bool>
{
    private readonly ILogger<VerifyOAuthTokenQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IGoogleOAuthTokenRepository _googleOAuthTokenRepository;

    public VerifyOAuthTokenQueryHandler(ILogger<VerifyOAuthTokenQueryHandler> logger,
        IUserContext userContext,
        IGoogleOAuthTokenRepository googleOAuthTokenRepository)
    {
        _logger = logger;
        _userContext = userContext;
        _googleOAuthTokenRepository = googleOAuthTokenRepository;
    }

    public async Task<bool> Handle(VerifyOAuthTokenQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling VerifyOAuthTokenQuery for user");

        var user = _userContext.GetCurrentUser();
        if (user == null)
            throw new UnauthorizedException("User is not authenticated");

        var token = await _googleOAuthTokenRepository.GetTokenByUserIdAsync(Guid.Parse(user.id));
        if (token == null || token.Expiry < DateTime.UtcNow)
            throw new NotFoundAuthOTokenException("Not found Google OAuth token for user or token expired");

        return true;
    }
}
