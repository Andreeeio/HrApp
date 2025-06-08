using HrApp.Domain.Constants;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HrAppContext _dbContext;

    public UserRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserAsync(string email)
    {
        var user = await _dbContext.User.Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == email);

        return user;
    }

    public async Task<User?> GetUserAsync(Guid id)
    {
        var user = await _dbContext.User
            .Include(u => u.Roles)
            .Include(u => u.Paid)
            .FirstOrDefaultAsync(u => u.Id == id);

        return user;
    }

    public async Task<bool> IfUserExistAsync(string email)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);
        return user != null;
    }

    public async Task<bool> IfUserExistAsync(Guid id)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == id);
        return user != null;
    }

    public async Task CreateUserAsync(User user)
    {
        var role = await _dbContext.Role.FirstOrDefaultAsync(x => x.Name == "user");
        user.Roles = [role!];
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetUserInTeamAsync(Guid? teamId)
    {
        var users = await _dbContext.User
            .Include(u => u.Roles)
            .Include(p => p.Paid)
            .Where(u => u.TeamId == teamId)
            .ToListAsync();

        return users;
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == id);
        if (user != null)
        {
            _dbContext.User.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
        return;
    }

    public async Task<List<Role>> GetUserRolesAsync(string email)
    {
        var user = await _dbContext.User
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return new List<Role>();
        
        return user.Roles;
    }

    public async Task AddRolesForUserAsync(string email, List<string> roleNames)
    {
        var user = await _dbContext.User
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == email);

        var rolesToAdd = await _dbContext.Role
            .Where(r => roleNames.Contains(r.Name)) 
            .ToListAsync();

        user!.Roles.Clear();

        user.Roles.AddRange(rolesToAdd);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetUserWithRolesAsync(List<string> roles)
    {
        return await _dbContext.User
            .Include(u => u.Roles) 
            .Where(u => u.Roles.Any(r => roles.Contains(r.Name)))
            .ToListAsync();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _dbContext.User
            .Where(u => !u.Roles.Any(r => r.Name == Roles.User.ToString() || r.Name == Roles.Ceo.ToString()))
            .Include(u => u.Roles)
            .Include(u => u.Paid)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
