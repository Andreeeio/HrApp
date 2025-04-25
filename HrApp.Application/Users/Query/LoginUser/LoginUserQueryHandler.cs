using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace HrApp.Application.Users.Query.LoginUser;

public class LoginUserQueryHandler(ILogger<LoginUserQueryHandler> logger,
    IUserRepository userRepository) : IRequestHandler<LoginUserQuery>
{
    private readonly ILogger<LoginUserQueryHandler> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    public async Task Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("User tring to log in");

        var user = await _userRepository.GetUserByEmail(request.Email)
            ?? throw new Exception("Not git");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

        if (!computeHash.SequenceEqual(user.PasswordHash))
        {
            throw new Exception("Invalid login or password");
        }
    }
}
