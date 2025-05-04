using HrApp.Domain.Entities;

namespace HrApp.Application.Users.DTO;

public class UserDTO
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsEmailConfirmed { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public List<Role> Roles { get; set; } = default!;
    public List<Leave> Leaves { get; set; } = default!;
    public List<EmploymentHistory> EmploymentHistories { get; set; } = default!;
    public List<WorkLog> WorkLogs { get; set; } = default!;
    public virtual Team? TeamLeader { get; set; }
    public virtual Team? Team { get; set; } = default!;
    public List<SalaryHistory> SalaryHistory { get; set; } = default!;
    public List<WorkedHoursRaport> WorkedHoursRaports { get; set; } = default!;
    public virtual Paid Paid { get; set; } = default!;
    public List<ExellImport> ExellImports { get; set; } = default!;
    public List<EmployeeRate> EmployeeRates { get; set; } = default!;
    public virtual EmployeeRate? Rater { get; set; } = default!;
}
