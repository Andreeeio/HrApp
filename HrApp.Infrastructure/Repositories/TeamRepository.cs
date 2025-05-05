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

    public async Task<List<Team>> GetAllTeams()
    {
        return await dbContext.Team.ToListAsync();
    }

    public async Task<List<Team>> GetAllTeamsForDepartment(Guid departmentId)
    {
        return await dbContext.Team
            .Where(t => t.DepartmentId == departmentId)
            .ToListAsync();
    }

    public async Task<Team?> GetTeamForUser(Guid userid)
    {
        return await dbContext.Team
            .Include(t => t.Employers) // Eager loading listy Employers
            .FirstOrDefaultAsync(t => t.Employers.Any(e => e.Id == userid));
    }

    public async Task CreateTeam(Team team)
    {
        await dbContext.Team.AddAsync(team);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddEmployer(Guid teamid, Guid userid)
    {
        var team = await dbContext.Team
            .Include(t => t.Employers) // Eager loading listy Employers
            .FirstOrDefaultAsync(t => t.Id == teamid);
        var user = await dbContext.User.FindAsync(userid);
        if (team != null && user != null)
        {
            team.Employers.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }


}
