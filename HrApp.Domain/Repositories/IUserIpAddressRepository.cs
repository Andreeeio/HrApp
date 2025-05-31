using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IUserIpAddressRepository
{
    public Task<bool> IfUserIpAddressExistAsync(Guid userId, string IpAddress);
    public Task AddUserIpAddressAsync(UserIpAddress ipAddress);
    public Task<List<UserIpAddress>> GetUserIpAddressesAsync(Guid userId);
    public Task SaveChangesAsync();
}
