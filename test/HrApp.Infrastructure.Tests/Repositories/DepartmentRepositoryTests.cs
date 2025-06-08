using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrApp.Domain.Entities;
using HrApp.Infrastructure.Presistance;
using HrApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class DepartmentRepositoryTests
{
    private readonly HrAppContext _context;
    private readonly DepartmentRepository _repository;

    public DepartmentRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<HrAppContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new HrAppContext(options);
        _repository = new DepartmentRepository(_context);
    }

    private Department CreateTestDepartment(Guid? id = null)
    {
        return new Department
        {
            Id = id ?? Guid.NewGuid(),
            Name = "IT Department",
            HeadOfDepartmentId = Guid.NewGuid(),
            TeamTag = "IT",
            Teams = new List<Team>()
        };
    }

    [Fact]
    public async Task CreateDepartmentAsync_ShouldAddDepartment()
    {
        var department = CreateTestDepartment();

        await _repository.CreateDepartmentAsync(department);

        var result = await _context.Department.FindAsync(department.Id);
        Assert.NotNull(result);
        Assert.Equal("IT Department", result.Name);
    }

    [Fact]
    public async Task GetAllDepartmentsAsync_ShouldReturnAllDepartments()
    {
        var department1 = CreateTestDepartment();
        var department2 = CreateTestDepartment();

        await _context.Department.AddRangeAsync(department1, department2);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllDepartmentsAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, d => d.Id == department1.Id);
        Assert.Contains(result, d => d.Id == department2.Id);
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_ShouldReturnCorrectDepartment()
    {
        var department = CreateTestDepartment();
        await _context.Department.AddAsync(department);
        await _context.SaveChangesAsync();

        var result = await _repository.GetDepartmentByIdAsync(department.Id);

        Assert.NotNull(result);
        Assert.Equal(department.Id, result.Id);
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var result = await _repository.GetDepartmentByIdAsync(Guid.NewGuid());
        Assert.Null(result);
    }

    [Fact]
    public async Task CountDepartmentAsync_ShouldReturnCorrectCount()
    {
        var department1 = CreateTestDepartment();
        var department2 = CreateTestDepartment();
        await _context.Department.AddRangeAsync(department1, department2);
        await _context.SaveChangesAsync();

        var count = await _repository.CountDepartmentAsync();

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task DeleteDepartmentAsync_ShouldRemoveDepartment()
    {
        var department = CreateTestDepartment();
        await _context.Department.AddAsync(department);
        await _context.SaveChangesAsync();

        await _repository.DeleteDepartmentAsync(department.Id);

        var result = await _context.Department.FindAsync(department.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteDepartmentAsync_ShouldAlsoRemoveTeams()
    {
        var department = CreateTestDepartment();
        var team = new Team
        {
            Id = Guid.NewGuid(),
            Name = "Team 1",
            DepartmentId = department.Id,
            TeamLeaderId = Guid.NewGuid()
        };

        department.Teams.Add(team);

        await _context.Team.AddAsync(team);
        await _context.Department.AddAsync(department);
        await _context.SaveChangesAsync();

        await _repository.DeleteDepartmentAsync(department.Id);

        var deletedDepartment = await _context.Department.FindAsync(department.Id);
        var deletedTeam = await _context.Team.FindAsync(team.Id);

        Assert.Null(deletedDepartment);
        Assert.Null(deletedTeam);
    }
}
