using HrApp.Domain.Constants;
using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IOverallRaportRepository
{
    public Task<List<User>> GetUserToCreateRaportAsync();
    public Task<List<Team>> GetTeamToCreateRaportAsync();
    public Task<List<Assignment>> GetAssignmentToCreateRaportAsync();
    public Task CreateRaportAsync(OverallRaport raport);
    public Task CreateRaportAsync(List<UserRaport> user);
    public Task CreateRaportAsync(List<TeamRaport> team);
    public Task CreateRaportAsync(List<AssignmentRaport> assignment);
    public Task UpdateAsync(List<UserRaport> userRaports);

}
