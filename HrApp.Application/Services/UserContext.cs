using HrApp.Application.Interfaces;
using HrApp.Application.Users.DTO;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HrApp.Application.Services;

public class UserContext(IHttpContextAccessor httpContext) : IUserContext
{
    private readonly IHttpContextAccessor _httpContext = httpContext;
    public CurrentUser? GetCurrentUser()
    {
        var user = (_httpContext?.HttpContext?.User)
            ?? throw new ArgumentNullException(nameof(_httpContext.HttpContext.User));

        if (user.Identity == null)
            return null;

        var id = user.FindFirst(u => u.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(u => u.Type == ClaimTypes.Email)!.Value;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value).ToList();

        return new CurrentUser(id, email, roles);
    }
}
