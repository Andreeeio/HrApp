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

    public async Task CreateTeam(Team team)
    {
        await dbContext.Team.AddAsync(team);
        await dbContext.SaveChangesAsync();
    }
}
