using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IAuthorizationRepository
{
    Task<Authorization?> GetUserAuthorizationAsync(Guid userId);
    Task AddAuthorizationAsync(Authorization authorization);
    Task SaveChangesAsync();
    Task RemoveAuthorizationAsync(Authorization authorization);
}
