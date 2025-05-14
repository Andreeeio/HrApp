using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByEmail(string email);
    public Task<bool> IfUserExist(string email);
    public Task<bool> IfUserExist(Guid id);
    public Task CreateUser(User user);
    public Task<List<User>> GetUserInTeamAsync(Guid? teamId);
    public Task<User?> GetUserById(Guid id);
    public Task DeleteUser(Guid id);
    public Task<List<Role>> GetUserRoles(string email);
    public Task AddRolesForUser(string email, List<string> strings);
}
