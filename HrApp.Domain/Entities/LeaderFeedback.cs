namespace HrApp.Domain.Entities;

public class LeaderFeedback
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public virtual Task Task { get; set; } = default!;
    public string Feedback { get; set; } = default!;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}
