using HrApp.Domain.Entities;

namespace HrApp.Application.Assignment.DTO;

public class AssignmentDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid AssignedToTeamId { get; set; }
    public int DifficultyLevel { get; set; }
    public List<LeaderFeedback> LeaderFeedbacks { get; set; } = new List<LeaderFeedback>();
    public List<AssignmentNotification> AssignmentNotifications { get; set; } = new List<AssignmentNotification>();

}