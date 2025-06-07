using HrApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HrApp.Application.Services;

public class GoogleAuthService(IConfiguration configuration) : IGoogleAuthService
{
    private readonly IConfiguration _configuration = configuration;
    public string GetAuthorizationUrl()
    {
        var clientId = _configuration["GoogleAPI:client_id"];
        var redirectUri = _configuration["GoogleAPI:redirect_uri"];
        var scope = "https://www.googleapis.com/auth/calendar";

        return $"https://accounts.google.com/o/oauth2/v2/auth" +
               $"?response_type=code" +
               $"&client_id={clientId}" +
               $"&redirect_uri={redirectUri}" +
               $"&scope={scope}" +
               $"&access_type=offline" +
               $"&prompt=consent";
    }
}
