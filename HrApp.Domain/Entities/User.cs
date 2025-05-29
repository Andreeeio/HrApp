namespace HrApp.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsEmailConfirmed { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string? ConfirmationToken { get; set; }
    public DateTime? ConfirmationTokenExpiration { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiration { get; set; }
    public byte[] PasswordHash { get; set; } = default!;
    public byte[] PasswordSalt { get; set; } = default!;
    public List<Role> Roles { get; set; } = default!;
    public List<Leave> Leaves { get; set; } = default!;
    public List<EmploymentHistory> EmploymentHistories { get; set; } = default!;
    public List<WorkLog> WorkLogs { get; set; } = default!;
    public virtual Authorization Authorization { get; set; } = default!;
    public Guid? TeamLeaderId { get; set; }  
    public virtual Team? TeamLeader { get; set; }
    public Guid? TeamId { get; set; }  
    public virtual Team? Team { get; set; } = default!;
    public List<SalaryHistory> SalaryHistory { get; set; } = default!;
    public virtual Department Department { get; set; } = default!;
    public List<WorkedHoursRaport> WorkedHoursRaports { get; set; } = default!;
    public virtual Paid Paid { get; set; } = default!;
    public List<ExellImport> ExellImports { get; set; } = default!;
    public List<EmployeeRate> EmployeeRates { get; set; } = default!;
    public List<EmployeeRate> Rater { get; set; } = default!;
    public List<WorkLogExportHistory> ExportedWorkLogs { get; set; } = new();
    public List<WorkLogExportHistory> ReceivedExportedWorkLogs { get; set; } = new();
    public List<GoogleOAuthToken> GoogleOAuthTokens { get; set; } = default!;
}
