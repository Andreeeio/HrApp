namespace HrApp.Domain.Entities;

public class WorkLog
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Hours { get; set; }
}
