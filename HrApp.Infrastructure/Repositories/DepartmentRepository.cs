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

    public async Task DeleteDepartment(Guid departmentId)
    {
        var department = await dbContext.Department
            .Include(d => d.Teams) // Załaduj powiązane zespoły
            .ThenInclude(t => t.Employers) // Załaduj powiązanych użytkowników w zespołach
            .FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department != null)
        {
            dbContext.Department.Remove(department); // Usunięcie departamentu
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<Department?> GetDepartmentById(Guid departmentId)
    {
        return await dbContext.Department
            .FirstOrDefaultAsync(d => d.Id == departmentId);
    }

    public async Task<int> CountDepartment()
    {
        return await dbContext.Department.CountAsync();
    }
}