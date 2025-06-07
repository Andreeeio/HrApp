using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using HrApp.Infrastructure.Repositories;

public class UserRepositoryTests
{
    private readonly IUserRepository _userRepository;
    private readonly HrAppContext _context;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<HrAppContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;

        _context = new HrAppContext(options);

        _userRepository = new UserRepository(_context);
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = new byte[] { 1, 2, 3 },  // przykładowe dane
            PasswordSalt = new byte[] { 4, 5, 6 },
            Roles = new List<Role>()
        };

        _context.User.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userRepository.GetUserAsync("test@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }
    [Fact]
    public async Task GetUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Act
        var result = await _userRepository.GetUserAsync("noone@example.com");

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public async Task GetUserAsync_ByEmail_ShouldReturnUser_WhenUserExists()
    {
        var user = CreateTestUser();
        _context.User.Add(user);
        await _context.SaveChangesAsync();

        var result = await _userRepository.GetUserAsync(user.Email);

        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
    }
    [Fact]
    public async Task GetUserAsync_ByEmail_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var result = await _userRepository.GetUserAsync("noone@example.com");

        Assert.Null(result);
    }
    [Fact]
    public async Task IfUserExistAsync_ByEmail_ShouldReturnTrue_WhenExists()
    {
        var user = CreateTestUser();
        _context.User.Add(user);
        await _context.SaveChangesAsync();

        var exists = await _userRepository.IfUserExistAsync(user.Email);

        Assert.True(exists);
    }

    [Fact]
    public async Task IfUserExistAsync_ByEmail_ShouldReturnFalse_WhenNotExists()
    {
        var exists = await _userRepository.IfUserExistAsync("noone@example.com");

        Assert.False(exists);
    }
    [Fact]
    public async Task CreateUserAsync_ShouldAddUserWithUserRole()
    {
        var role = new Role { Id = Guid.NewGuid(), Name = "user" };
        _context.Role.Add(role);
        await _context.SaveChangesAsync();

        var user = CreateTestUser();

        await _userRepository.CreateUserAsync(user);

        var result = await _context.User.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == user.Email);
        Assert.NotNull(result);
        Assert.Contains(result.Roles, r => r.Name == "user");
    }
    [Fact]
    public async Task DeleteUserAsync_ShouldRemoveUser()
    {
        var user = CreateTestUser();
        _context.User.Add(user);
        await _context.SaveChangesAsync();

        await _userRepository.DeleteUserAsync(user.Id);

        var result = await _context.User.FindAsync(user.Id);
        Assert.Null(result);
    }
    [Fact]
    public async Task GetUserRolesAsync_ShouldReturnRoles_WhenUserHasRoles()
    {
        var user = CreateTestUser();
        var role = new Role { Id = Guid.NewGuid(), Name = "admin" };
        user.Roles.Add(role);
        _context.User.Add(user);
        await _context.SaveChangesAsync();

        var roles = await _userRepository.GetUserRolesAsync(user.Email);

        Assert.Contains(roles, r => r.Name == "admin");
    }
    [Fact]
    public async Task AddRolesForUserAsync_ShouldReplaceRoles()
    {
        var user = CreateTestUser();
        var roleOld = new Role { Id = Guid.NewGuid(), Name = "oldrole" };
        var roleNew = new Role { Id = Guid.NewGuid(), Name = "newrole" };

        _context.Role.AddRange(roleOld, roleNew);
        user.Roles.Add(roleOld);
        _context.User.Add(user);
        await _context.SaveChangesAsync();

        await _userRepository.AddRolesForUserAsync(user.Email, new List<string> { "newrole" });

        var updatedUser = await _context.User.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == user.Email);
        Assert.Single(updatedUser.Roles);
        Assert.Equal("newrole", updatedUser.Roles.First().Name);
    }
    [Fact]
    public async Task GetUserInTeamAsync_ShouldReturnUsersInTeam()
    {
        var teamId = Guid.NewGuid();
        var user = CreateTestUser();
        user.TeamId = teamId;

        _context.User.Add(user);
        await _context.SaveChangesAsync();

        var result = await _userRepository.GetUserInTeamAsync(teamId);

        Assert.Single(result);
        Assert.Equal(teamId, result.First().TeamId);
    }
    [Fact]
    public async Task GetUserWithRolesAsync_ShouldReturnUsersWithGivenRole()
    {
        var role = new Role { Id = Guid.NewGuid(), Name = "manager" };
        var user = CreateTestUser();
        user.Roles.Add(role);

        _context.Role.Add(role);
        _context.User.Add(user);
        await _context.SaveChangesAsync();

        var users = await _userRepository.GetUserWithRolesAsync(new List<string> { "manager" });

        Assert.Single(users);
        Assert.Equal("manager", users.First().Roles.First().Name);
    }
    private User CreateTestUser()
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = new byte[] { 1, 2, 3 },
            PasswordSalt = new byte[] { 4, 5, 6 },
            Roles = new List<Role>()
        };
    }
}
