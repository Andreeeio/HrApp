namespace HrApp.Application.Users.DTO;

public record CurrentUser(string id,
    string email,
    string twoFA,
    IEnumerable<string> roles)
{
    public bool IsInRole(string role)
    {
        return roles.Contains(role);
    }
}
