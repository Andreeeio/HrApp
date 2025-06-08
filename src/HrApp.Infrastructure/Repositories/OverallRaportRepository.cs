using DocumentFormat.OpenXml.Spreadsheet;
using HrApp.Domain.Constants;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class OverallRaportRepository : IOverallRaportRepository
{
    private readonly HrAppContext _dbContext;

    public OverallRaportRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> GetUserToCreateRaportAsync()
    {
        var user = await _dbContext.User
            .Where(r => r.Roles
                .Any(role => role.Name != Roles.User.ToString()))
            .Include(u => u.SalaryHistory
                .Where(s => s.MonthNYear.Year == DateTime.Now.Year))
            .ToListAsync();
        
        return user;
    }

    public async Task<List<Team>> GetTeamToCreateRaportAsync()
    {
        var teams = await _dbContext.Team
            .ToListAsync();

        return teams;
    }

    public async Task<List<Assignment>> GetAssignmentToCreateRaportAsync()
    {
        var assignments = await _dbContext.Assignment
            .ToListAsync();

        return assignments;
    }

    public async Task CreateRaportAsync(OverallRaport raport)
    {
        _dbContext.OverallRaport.Add(raport);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateRaportAsync(List<UserRaport> user)
    {
        _dbContext.UserRaport.AddRange(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateRaportAsync(List<TeamRaport> team)
    {
        _dbContext.TeamRaport.AddRange(team);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateRaportAsync(List<AssignmentRaport> assignment)
    {
        _dbContext.AssignmentRaport.AddRange(assignment);
        await _dbContext.SaveChangesAsync();
    }
}
