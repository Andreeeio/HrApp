namespace HrApp.Domain.Entities;

public class Task
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid AssignedToTeamId { get; set; }
    public virtual Team AssignedToTeam { get; set; } = default!;
    public int DifficultyLevel { get; set; }
    public Guid? LeaderFeedbackId { get; set; }
    public virtual LeaderFeedback? LeaderFeedback { get; set; }
}
