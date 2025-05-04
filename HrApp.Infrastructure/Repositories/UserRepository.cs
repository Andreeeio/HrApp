using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class UserRepository(HrAppContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await dbContext.User.Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<bool> IfUserExist(string email)
    {
        var user = await dbContext.User.FirstOrDefaultAsync(u => u.Email == email);
        return user != null;
    }

    public async Task CreateUser(User user)
    {
        var role = await dbContext.Role.FirstOrDefaultAsync(x => x.Name == "user");
        user.Roles = [role!];
        dbContext.User.Add(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetUserInTeamAsync(Guid teamId)
    {
        var users = await dbContext.User
            .Include(u => u.Roles)
            .Where(u => u.TeamId == teamId)
            .ToListAsync();

        return users;
    }
}
