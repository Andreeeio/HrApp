namespace HrApp.Domain.Entities;

public class Assignment
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid AssignedToTeamId { get; set; }
    public virtual Team AssignedToTeam { get; set; } = default!;
    public int DifficultyLevel { get; set; }
    public List<LeaderFeedback> LeaderFeedbacks { get; set; } = default!;
    public List<AssignmentNotification> AssignmentNotifications { get; set; } = default!;
}
