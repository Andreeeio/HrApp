using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IOverallRaportRepository
{
    Task<List<User>> GetUserToCreateRaportAsync();
    Task<List<Team>> GetTeamToCreateRaportAsync();
    Task<List<Assignment>> GetAssignmentToCreateRaportAsync();
    Task CreateRaportAsync(OverallRaport raport);
    Task CreateRaportAsync(List<UserRaport> user);
    Task CreateRaportAsync(List<TeamRaport> team);
    Task CreateRaportAsync(List<AssignmentRaport> assignment);
}