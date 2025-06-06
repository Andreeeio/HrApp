﻿namespace HrApp.Application.GoogleOAuthTokens.DTO;

public class GoogleOAuthTokenDTO
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime Expiry { get; set; }
}
