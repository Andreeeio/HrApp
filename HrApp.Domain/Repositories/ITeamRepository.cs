using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface ITeamRepository
{
    public Task<List<Team>> GetAllTeams();
    public Task<List<Team>> GetAllTeamsForDepartment(Guid departmentId);
    public Task CreateTeam(Team team);
}
