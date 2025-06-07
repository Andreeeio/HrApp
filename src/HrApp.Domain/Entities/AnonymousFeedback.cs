namespace HrApp.Domain.Entities;

public class AnonymousFeedback
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = default!;
    public string Message { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public Guid TeamId { get; set; }
    public virtual Team Team { get; set; } = default!;
}
