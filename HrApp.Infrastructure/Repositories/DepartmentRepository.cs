using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly HrAppContext dbContext;

    public DepartmentRepository(HrAppContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<List<Department>> GetAllDepartmentsAsync()
    {
        return await dbContext.Department.ToListAsync();
    }

    public async Task CreateDepartmentAsync(Department department)
    {
        await dbContext.Department.AddAsync(department);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteDepartmentAsync(Guid departmentId)
    {
        var department = await dbContext.Department
            .Include(d => d.Teams) 
            .ThenInclude(t => t.Employers) 
            .FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department != null)
        {
            dbContext.Department.Remove(department); 
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<Department?> GetDepartmentByIdAsync(Guid departmentId)
    {
        return await dbContext.Department
            .FirstOrDefaultAsync(d => d.Id == departmentId);
    }

    public async Task<int> CountDepartmentAsync()
    {
        return await dbContext.Department.CountAsync();
    }
}