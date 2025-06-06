using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class AuthorizationRepository : IAuthorizationRepository
{
    private readonly HrAppContext _dbContext;

    public AuthorizationRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Authorization?> GetUserAuthorizationAsync(Guid userId)
    {
        var authorization = await _dbContext.Authorization
            .FirstOrDefaultAsync(a => a.UserId == userId && a.IsActive == true);

        return authorization;
    }

    public async Task AddAuthorizationAsync(Authorization authorization)
    {
        _dbContext.Authorization.Add(authorization);
        await _dbContext.SaveChangesAsync();
    }
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAuthorizationAsync(Authorization authorization)
    {
        _dbContext.Authorization.Remove(authorization);
        await _dbContext.SaveChangesAsync();
    }
}
