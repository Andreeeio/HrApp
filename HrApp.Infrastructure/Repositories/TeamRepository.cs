using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly HrAppContext _dbContext;

    public TeamRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Team>> GetAllTeamsAsync()
    {
        return await _dbContext.Team.ToListAsync();
    }

    public async Task<List<Team>> GetAllTeamsForDepartmentAsync(Guid departmentId)
    {
        return await _dbContext.Team
            .Where(t => t.DepartmentId == departmentId)
            .ToListAsync();
    }

    public async Task<Team?> GetTeamForUserAsync(Guid userId)
    {
        return await _dbContext.Team
            .Include(t => t.Employers)
            .FirstOrDefaultAsync(t => t.Employers.Any(e => e.Id == userId) || t.TeamLeaderId == userId);
    }

    public async Task CreateTeamAsync(Team team)
    {
        _dbContext.Team.Add(team);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddEmployerAsync(Guid teamid, Guid userid)
    {
        var team = await _dbContext.Team
            .Include(t => t.Employers) 
            .FirstOrDefaultAsync(t => t.Id == teamid);

        var user = await _dbContext.User.FindAsync(userid);
        if (team != null && user != null)
        {
            team.Employers.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveEmployerAsync(Guid teamid, Guid userid)
    {
        var team = await _dbContext.Team
            .Include(t => t.Employers) 
            .FirstOrDefaultAsync(t => t.Id == teamid);

        var user = await _dbContext.User.FindAsync(userid);
        if (team != null && user != null)
        {
            team.Employers.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task DeleteTeamAsync(Guid teamId)
    {
        var team = await _dbContext.Team
            .Include(t => t.Employers)
            .FirstOrDefaultAsync(t => t.Id == teamId);

        if (team != null)
        {
            _dbContext.Team.Remove(team); 
            await _dbContext.SaveChangesAsync();
        }
    }
}
