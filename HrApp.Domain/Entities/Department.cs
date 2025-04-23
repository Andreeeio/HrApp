namespace HrApp.Domain.Entities;

public class Department
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<Team> Teams { get; set; } = default!;
    public Guid HeadOfDepartmentId { get; set; }
    public virtual User HeadOfDepartment { get; set; } = default!;
    public string TeamTag { get; set; } = default!;
}
