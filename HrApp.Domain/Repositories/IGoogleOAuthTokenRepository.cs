using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IGoogleOAuthTokenRepository
{
    public Task<GoogleOAuthToken?> GetTokenByUserIdAsync(Guid userId);
    public Task AddNewToken(GoogleOAuthToken newToken, GoogleOAuthToken? oldToken);
}
