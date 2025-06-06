using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllDepartmentsAsync();
    Task CreateDepartmentAsync(Department department);
    Task DeleteDepartmentAsync(Guid DepartmentId);
    Task<Department?> GetDepartmentByIdAsync(Guid departmentId);
    Task<int> CountDepartmentAsync();
}
