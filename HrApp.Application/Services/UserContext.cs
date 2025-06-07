using HrApp.Application.Interfaces;
using HrApp.Application.Users.DTO;
using HrApp.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace HrApp.Application.Services;

public class UserContext(IHttpContextAccessor httpContext) : IUserContext
{
    private readonly IHttpContextAccessor _httpContext = httpContext;
    public CurrentUser? GetCurrentUser()
    {
        var user = (_httpContext?.HttpContext?.User)
            ?? throw new BadRequestException(nameof(_httpContext.HttpContext.User));

        if (user.Identity == null || !user.Identity.IsAuthenticated)
            return null;

        var id = user.FindFirst(u => u.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(u => u.Type == ClaimTypes.Email)!.Value;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value).ToList();
        var twoFA = user.FindFirst(u => u.Type == "2FA_verf")!.Value;
        var ipVerification = user.FindFirst(u => u.Type == "IpVerification")!.Value;

        return new CurrentUser(id, email, twoFA, ipVerification, roles);
    }
}
