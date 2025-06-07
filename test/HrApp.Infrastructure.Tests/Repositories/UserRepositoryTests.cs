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
}
