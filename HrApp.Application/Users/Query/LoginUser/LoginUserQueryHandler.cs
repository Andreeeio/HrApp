using HrApp.Application.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace HrApp.Application.Users.Query.LoginUser;

public class LoginUserQueryHandler(ILogger<LoginUserQueryHandler> logger,
    IUserRepository userRepository,
    ITokenService tokenService) : IRequestHandler<LoginUserQuery,string>
{
    private readonly ILogger<LoginUserQueryHandler> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User tring to log in");

        var user = await _userRepository.GetUserByEmail(request.Email)
            ?? throw new Exception("Not git");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

        if (!computeHash.SequenceEqual(user.PasswordHash))
        {
            throw new Exception("Invalid login or password");
        }

        var token = _tokenService.GetToken(user);
        return token;
    }
}
