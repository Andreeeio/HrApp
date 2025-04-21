namespace HrApp.Domain.Entities;

public class TaskNotification
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public virtual Task Task { get; set; } = default!;
    public string NotificationMessage { get; set; } = default!;
    public string MessageType { get; set; } = default!;
    public DateTime SendDate { get; set; }
}
