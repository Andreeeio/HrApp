using HrApp.Domain.Entities;

namespace HrApp.Application.Department.DTO;

public class DepartmentDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<Team> Teams { get; set; } = default!;
    public Guid HeadOfDepartmentId { get; set; }
    public string TeamTag { get; set; } = default!;
}
