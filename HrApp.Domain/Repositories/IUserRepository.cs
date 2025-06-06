using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string email);
    Task<User?> GetUserAsync(Guid id);
    Task<bool> IfUserExistAsync(string email);
    Task<bool> IfUserExistAsync(Guid id);
    Task CreateUserAsync(User user);
    Task<List<User>> GetUserInTeamAsync(Guid? teamId);
    Task DeleteUserAsync(Guid id);
    Task<List<Role>> GetUserRolesAsync(string email);
    Task AddRolesForUserAsync(string email, List<string> strings);
    Task<List<User>> GetUserWithRolesAsync(List<string> roles);
    Task<List<User>> GetAllUsersAsync();
    Task SaveChangesAsync();
}
