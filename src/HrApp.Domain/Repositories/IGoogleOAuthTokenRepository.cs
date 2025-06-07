using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IGoogleOAuthTokenRepository
{
    Task<GoogleOAuthToken?> GetTokenByUserIdAsync(Guid userId);
    Task AddNewTokenAsync(GoogleOAuthToken newToken, GoogleOAuthToken? oldToken);
}
