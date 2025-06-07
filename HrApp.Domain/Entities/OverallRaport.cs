namespace HrApp.Domain.Entities;

public class OverallRaport
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<UserRaport> UserRaport { get; set; } = default!;
    public List<TeamRaport> TeamRaport { get; set; } = default!;
    public List<AssignmentRaport> AssignmentRaport { get; set; } = default!;
    public DateOnly BackupDate { get; set; }
}
