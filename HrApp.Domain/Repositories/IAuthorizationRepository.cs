using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IAuthorizationRepository
{
    public Task<Authorization?> GetUserAuthorizationAsync(Guid userId);
    public Task AddAuthorization(Authorization authorization);
    public Task SaveChangesAsync();
    public Task RemoveAuthorization(Authorization authorization);

}
