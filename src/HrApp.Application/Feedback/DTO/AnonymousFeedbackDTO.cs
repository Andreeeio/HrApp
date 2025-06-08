namespace HrApp.Application.Feedback.DTO;

public class AnonymousFeedbackDTO
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = default!;
    public string Message { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public Guid TeamId { get; set; }
}
