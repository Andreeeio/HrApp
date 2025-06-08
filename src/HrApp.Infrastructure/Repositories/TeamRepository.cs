using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly HrAppContext dbContext;

    public TeamRepository(HrAppContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Team>> GetAllTeamsAsync()
    {
        return await dbContext.Team.ToListAsync();
    }

    public async Task<List<Team>> GetAllTeamsForDepartmentAsync(Guid departmentId)
    {
        return await dbContext.Team
            .Where(t => t.DepartmentId == departmentId)
            .ToListAsync();
    }

    public async Task<Team?> GetTeamForUserAsync(Guid userId)
    {
        return await dbContext.Team
            .Include(t => t.Employers)
            .FirstOrDefaultAsync(t => t.Employers.Any(e => e.Id == userId) || t.TeamLeaderId == userId);
    }

    public async Task CreateTeamAsync(Team team)
    {
        dbContext.Team.Add(team);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddEmployerAsync(Guid teamid, Guid userid)
    {
        var team = await dbContext.Team
            .Include(t => t.Employers) 
            .FirstOrDefaultAsync(t => t.Id == teamid);

        var user = await dbContext.User.FindAsync(userid);
        if (team != null && user != null)
        {
            team.Employers.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveEmployerAsync(Guid teamid, Guid userid)
    {
        var team = await dbContext.Team
            .Include(t => t.Employers) 
            .FirstOrDefaultAsync(t => t.Id == teamid);

        var user = await dbContext.User.FindAsync(userid);
        if (team != null && user != null)
        {
            team.Employers.Remove(user);
            await dbContext.SaveChangesAsync();
        }
    }
    public async Task DeleteTeamAsync(Guid teamId)
    {
        var team = await dbContext.Team
            .Include(t => t.Employers)
            .FirstOrDefaultAsync(t => t.Id == teamId);

        if (team != null)
        {
            dbContext.Team.Remove(team); 
            await dbContext.SaveChangesAsync();
        }
    }
}
