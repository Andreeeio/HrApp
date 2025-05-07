using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface ITeamRepository
{
    public Task<List<Team>> GetAllTeams();
    public Task<List<Team>> GetAllTeamsForDepartment(Guid departmentId);
    public Task<Team> GetTeamForUser(Guid userid);
    public Task CreateTeam(Team team);
    public Task AddEmployer(Guid teamid, Guid userid);
    public Task RemoveEmployer(Guid teamid, Guid userid);
    public Task DeleteTeam(Guid teamid);
}
