namespace HrApp.Domain.Entities;

public class UserIpAddress 
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public string IpAddress { get; set; } = default!;
    public string UserAgent { get; set; } = default!;
    public DateTime LastAccessed { get; set; }
    public bool IsActive { get; set; } = true;
}
