using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface ITeamRepository
{
    public Task<List<Team>> GetAllTeams();
}
