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

    public async Task<bool> IfUserExist(Guid id)
    {
        var user = await dbContext.User.FirstOrDefaultAsync(u => u.Id == id);
        return user != null;
    }

    public async Task CreateUser(User user)
    {
        var role = await dbContext.Role.FirstOrDefaultAsync(x => x.Name == "user");
        user.Roles = [role!];
        dbContext.User.Add(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetUserInTeamAsync(Guid? teamId)
    {
        var users = await dbContext.User
            .Include(u => u.Roles)
            .Where(u => u.TeamId == teamId)
            .ToListAsync();

        return users;
    }

    public async Task<User?> GetUserById(Guid id) // Changed parameter type from string to Guid
    {
        var user = await dbContext.User.Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id); // No changes needed here as 'u.Id' is of type Guid
        return user;
    }

    public async Task DeleteUser(Guid id)
    {
        await dbContext.User
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync();
        return;
    }

    public async Task<List<Role>> GetUserRoles(string email)
    {
        var user = await dbContext.User
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return new List<Role>();
        
        return user.Roles;
    }

    public async Task AddRolesForUser(string email, List<string> roleNames)
    {
        var user = await dbContext.User
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == email);

        var rolesToAdd = await dbContext.Role
            .Where(r => roleNames.Contains(r.Name)) 
            .ToListAsync();

        user!.Roles.Clear();

        user.Roles.AddRange(rolesToAdd);

        await dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetUserWithRolesAsync(List<string> roles)
    {
        return await dbContext.User
            .Include(u => u.Roles) 
            .Where(u => u.Roles.Any(r => roles.Contains(r.Name)))
            .ToListAsync();
    }
    public async Task<User> GetByIdAsync(Guid id)
    {
        return await dbContext.User
            .Include(u => u.Roles) 
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}
