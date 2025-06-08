namespace HrApp.Domain.Entities;

public class GoogleOAuthToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } = default!;
    public virtual User User { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime Expiry { get; set; }
}
