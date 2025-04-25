using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByEmail(string email);
    public Task<bool> IfUserExist(string email);
    public Task CreateUser(User user);

}
