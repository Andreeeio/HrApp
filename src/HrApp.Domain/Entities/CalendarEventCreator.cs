namespace HrApp.Domain.Entities;

public class CalendarEventCreator
{
    public Guid Id { get; set; }
    public virtual Calendar Calendar { get; set; } = default!;
    public Guid CalendarId { get; set; }
    public string Email { get; set; } = default!;
}
