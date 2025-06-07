namespace HrApp.Domain.Entities;

public class AssignmentRaport
{
    public Guid Id { get; set; }
    public Guid AssignmentId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsEnded { get; set; }
    public Guid? AssignedToTeamId { get; set; }
    public int DifficultyLevel { get; set; }
    public Guid OverallRaportId { get; set; }
    public virtual OverallRaport OverallRaport { get; set; } = default!;
}
