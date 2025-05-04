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
    public async Task<List<Department>> GetAllDepartments()
    {
        return await dbContext.Department.ToListAsync();
    }

    public async Task CreateDepartment(Department department)
    {
        await dbContext.Department.AddAsync(department);
        await dbContext.SaveChangesAsync();
    }
}