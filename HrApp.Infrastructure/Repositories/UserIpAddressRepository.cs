using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class UserIpAddressRepository(HrAppContext dbContext) : IUserIpAddressRepository
{
    private readonly HrAppContext _dbContext = dbContext;
    public async Task AddUserIpAddressAsync(UserIpAddress ipAddress)
    {
        _dbContext.UserIpAddress.Add(ipAddress);
        await _dbContext.SaveChangesAsync();
    }

    public Task<List<UserIpAddress>> GetUserIpAddressesAsync(Guid userId)
    {
        return _dbContext.UserIpAddress
            .Where(x => x.UserId == userId && x.IsActive == true)
            .ToListAsync();
    }

    public async Task<bool> IfUserIpAddressExistAsync(Guid userId, string IpAddress)
    {
        return await _dbContext.UserIpAddress
            .AnyAsync(x => x.UserId == userId && x.IpAddress == IpAddress && x.IsActive == true);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
