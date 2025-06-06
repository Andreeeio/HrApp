using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class GoogleOAuthTokenRepository(HrAppContext dbContext) : IGoogleOAuthTokenRepository
{
    private readonly HrAppContext _dbContext = dbContext;

    public async Task AddNewTokenAsync(GoogleOAuthToken newToken, GoogleOAuthToken? oldToken)
    {
        if (oldToken != null)
            _dbContext.GoogleOAuthToken.Remove(oldToken);

        _dbContext.GoogleOAuthToken.Add(newToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<GoogleOAuthToken?> GetTokenByUserIdAsync(Guid userId)
    {
        var token = await _dbContext.GoogleOAuthToken.FirstOrDefaultAsync(t => t.UserId == userId);
        return token;
    }
}
