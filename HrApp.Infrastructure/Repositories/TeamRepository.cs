using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class TeamRepository(HrAppContext dbContext) : ITeamRepository
{
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
}
