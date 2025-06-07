using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface ITeamRepository
{
    Task<List<Team>> GetAllTeamsAsync();
    Task<List<Team>> GetAllTeamsForDepartmentAsync(Guid departmentId);
    Task<Team?> GetTeamForUserAsync(Guid userid);
    Task CreateTeamAsync(Team team);
    Task AddEmployerAsync(Guid teamid, Guid userid);
    Task RemoveEmployerAsync(Guid teamid, Guid userid);
    Task DeleteTeamAsync(Guid teamid);
}
