using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IUserIpAddressRepository
{
    Task<bool> IfUserIpAddressExistAsync(Guid userId, string IpAddress);
    Task AddUserIpAddressAsync(UserIpAddress ipAddress);
    Task<List<UserIpAddress>> GetUserIpAddressesAsync(Guid userId);
    Task DeleteUserIpAddressAsync(Guid userId);
    Task SaveChangesAsync();
}
