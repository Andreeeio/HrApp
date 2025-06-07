namespace HrApp.Domain.Entities;

public class AssignmentNotification
{
    public Guid Id { get; set; }
    public Guid AssignmentId { get; set; }
    public virtual Assignment Assignment { get; set; } = default!;
    public string MessageType { get; set; } = default!;
    public string NotificationMessage { get; set; } = default!;
    public DateTime SendDate { get; set; }
}
