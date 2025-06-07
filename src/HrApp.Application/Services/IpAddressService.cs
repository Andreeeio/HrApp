using HrApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HrApp.Application.Services;

public class IpAddressService(IHttpContextAccessor httpContextAccessor) : IIpAddressService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GetUserAgent()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context == null)
            throw new InvalidOperationException("HTTP context or connection is not available.");

        if (context.Request.Headers.TryGetValue("User-Agent", out var userAgentValues))
        {
            var userAgent = userAgentValues.ToString();
            return string.IsNullOrEmpty(userAgent) ? "unknown" : userAgent;
        }
        else
            return "unknown";
    }

    public string GetUserIpAddress()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context == null)
            throw new InvalidOperationException("HTTP context or connection is not available.");
        

        var ip = context.Connection.RemoteIpAddress?.ToString();

        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            var forwardedIp = forwardedFor.FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedIp))
                ip = forwardedIp.Split(',').FirstOrDefault()?.Trim();
        }

        return ip ?? "unknown";
    }
}
