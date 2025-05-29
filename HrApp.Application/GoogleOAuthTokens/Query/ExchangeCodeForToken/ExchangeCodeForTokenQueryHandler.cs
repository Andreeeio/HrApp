using DotNetEnv;
using Google.Apis.Auth.OAuth2.Responses;
using HrApp.Application.GoogleOAuthTokens.Query.DTO;
using HrApp.Application.Interfaces;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HrApp.Application.GoogleOAuthTokens.Query.ExchangeCodeForToken;

public class ExchangeCodeForTokenQueryHandler(ILogger<ExchangeCodeForTokenQueryHandler> logger,
    IUserContext userContext,
    IGoogleOAuthTokenRepository googleOAuthTokenRepository,
    IConfiguration configuration) : IRequestHandler<ExchangeCodeForTokenQuery>
{
    private readonly ILogger<ExchangeCodeForTokenQueryHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;
    private readonly IGoogleOAuthTokenRepository _googleOAuthTokenRepository = googleOAuthTokenRepository;
    private readonly IConfiguration _configuration = configuration;
    public async Task Handle(ExchangeCodeForTokenQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling ExchangeCodeForTokenQuery for user");

        Env.Load();

        var user = _userContext.GetCurrentUser();
        if (user is null)
            throw new UnauthorizedException("User not found in context");
        

        var clientId = _configuration["GoogleAPI:client_id"];
        var clientSecret = Env.GetString("CLIENT_SECRET");
        var redirectUri = _configuration["GoogleAPI:redirect_uri"];

        using var http = new HttpClient();

        var response = await http.PostAsync("https://oauth2.googleapis.com/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"code", request.Code},
                {"client_id", clientId!},
                {"client_secret", clientSecret},
                {"redirect_uri", redirectUri!},
                {"grant_type", "authorization_code"}
            }));

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<TokenResponse>(json)!;

        var token = new GoogleOAuthToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(user.id),
            AccessToken = result.access_token,
            RefreshToken = result.refresh_token,
            Expiry = DateTime.UtcNow.AddSeconds(result.expires_in)
        };

        var existing = await _googleOAuthTokenRepository.GetTokenByUserIdAsync(token.UserId);
        
        await _googleOAuthTokenRepository.AddNewToken(token, existing);
    }

    private class TokenResponse
    {
        public string access_token { get; set; } = default!;
        public string refresh_token { get; set; } = default!;
        public int expires_in { get; set; }
    }
}
