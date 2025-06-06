using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly HrAppContext _dbContext;

    public DepartmentRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Department>> GetAllDepartmentsAsync()
    {
        return await _dbContext.Department.ToListAsync();
    }

    public async Task CreateDepartmentAsync(Department department)
    {
        _dbContext.Department.Add(department);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteDepartmentAsync(Guid departmentId)
    {
        var department = await _dbContext.Department
            .Include(d => d.Teams) 
            .ThenInclude(t => t.Employers) 
            .FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department != null)
        {
            _dbContext.Department.Remove(department); 
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<Department?> GetDepartmentByIdAsync(Guid departmentId)
    {
        return await _dbContext.Department
            .FirstOrDefaultAsync(d => d.Id == departmentId);
    }

    public async Task<int> CountDepartmentAsync()
    {
        return await _dbContext.Department.CountAsync();
    }
}