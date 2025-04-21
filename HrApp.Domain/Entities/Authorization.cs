namespace HrApp.Domain.Entities;

public class Authorization
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public int VerificationCode { get; set; }
    public DateTime VerificationCodeExpiration { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; }
    public int AttemptCount { get; set; }
    public bool IsActive { get; set; }
}
