using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrApp.Domain.Entities;
using HrApp.Infrastructure.Presistance;
using HrApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class TeamRepositoryTests
{
    private readonly HrAppContext _context;
    private readonly TeamRepository _repository;

    public TeamRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<HrAppContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new HrAppContext(options);
        _repository = new TeamRepository(_context);
    }

    private Team CreateTestTeam(Guid? departmentId = null, Guid? teamLeaderId = null)
    {
        return new Team
        {
            Id = Guid.NewGuid(),
            Name = "Test Team",
            DepartmentId = departmentId ?? Guid.NewGuid(),
            TeamLeaderId = teamLeaderId ?? Guid.NewGuid(),
            Employers = new List<User>()
        };
    }

    private User CreateTestUser(Guid? id = null)
    {
        return new User
        {
            Id = id ?? Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PasswordHash = new byte[] { 1 },
            PasswordSalt = new byte[] { 1 },
            Roles = new List<Role>()
        };
    }

    [Fact]
    public async Task CreateTeamAsync_ShouldAddTeam()
    {
        var team = CreateTestTeam();

        await _repository.CreateTeamAsync(team);

        var result = await _context.Team.FindAsync(team.Id);
        Assert.NotNull(result);
        Assert.Equal(team.Name, result.Name);
    }

    [Fact]
    public async Task GetAllTeamsAsync_ShouldReturnAllTeams()
    {
        var team1 = CreateTestTeam();
        var team2 = CreateTestTeam();

        await _context.Team.AddRangeAsync(team1, team2);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllTeamsAsync();

        Assert.NotNull(result);
        Assert.True(result.Count >= 2);
        Assert.Contains(result, t => t.Id == team1.Id);
        Assert.Contains(result, t => t.Id == team2.Id);
    }

    [Fact]
    public async Task GetAllTeamsForDepartmentAsync_ShouldReturnTeamsForDepartment()
    {
        var departmentId = Guid.NewGuid();
        var team1 = CreateTestTeam(departmentId: departmentId);
        var team2 = CreateTestTeam(departmentId: Guid.NewGuid());

        await _context.Team.AddRangeAsync(team1, team2);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllTeamsForDepartmentAsync(departmentId);

        Assert.Single(result);
        Assert.Equal(departmentId, result.First().DepartmentId);
    }

    [Fact]
    public async Task GetTeamForUserAsync_ShouldReturnTeam_WhenUserIsEmployer()
    {
        var user = CreateTestUser();
        var team = CreateTestTeam(teamLeaderId: Guid.NewGuid());
        team.Employers.Add(user);

        await _context.User.AddAsync(user);
        await _context.Team.AddAsync(team);
        await _context.SaveChangesAsync();

        var result = await _repository.GetTeamForUserAsync(user.Id);

        Assert.NotNull(result);
        Assert.Equal(team.Id, result.Id);
    }

    [Fact]
    public async Task GetTeamForUserAsync_ShouldReturnTeam_WhenUserIsTeamLeader()
    {
        var user = CreateTestUser();
        var team = CreateTestTeam(teamLeaderId: user.Id);

        await _context.User.AddAsync(user);
        await _context.Team.AddAsync(team);
        await _context.SaveChangesAsync();

        var result = await _repository.GetTeamForUserAsync(user.Id);

        Assert.NotNull(result);
        Assert.Equal(team.Id, result.Id);
    }

    [Fact]
    public async Task AddEmployerAsync_ShouldAddUserToTeam()
    {
        var team = CreateTestTeam();
        var user = CreateTestUser();

        await _context.Team.AddAsync(team);
        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync();

        await _repository.AddEmployerAsync(team.Id, user.Id);

        var updatedTeam = await _context.Team
            .Include(t => t.Employers)
            .FirstOrDefaultAsync(t => t.Id == team.Id);

        Assert.Contains(updatedTeam.Employers, e => e.Id == user.Id);
    }

    [Fact]
    public async Task RemoveEmployerAsync_ShouldRemoveUserFromTeam()
    {
        var team = CreateTestTeam();
        var user = CreateTestUser();

        team.Employers.Add(user);

        await _context.Team.AddAsync(team);
        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync();

        await _repository.RemoveEmployerAsync(team.Id, user.Id);

        var updatedTeam = await _context.Team
            .Include(t => t.Employers)
            .FirstOrDefaultAsync(t => t.Id == team.Id);

        Assert.DoesNotContain(updatedTeam.Employers, e => e.Id == user.Id);
    }

    [Fact]
    public async Task DeleteTeamAsync_ShouldRemoveTeam()
    {
        var team = CreateTestTeam();

        await _context.Team.AddAsync(team);
        await _context.SaveChangesAsync();

        await _repository.DeleteTeamAsync(team.Id);

        var deletedTeam = await _context.Team.FindAsync(team.Id);
        Assert.Null(deletedTeam);
    }
}
