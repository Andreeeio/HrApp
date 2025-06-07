using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HrApp.Application.Users.Command.FirstLoginUser;

public class FirstLoginUserCommandHandler : IRequestHandler<FirstLoginUserCommand,string>
{
    private readonly ILogger<FirstLoginUserCommandHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public FirstLoginUserCommandHandler(ILogger<FirstLoginUserCommandHandler> logger, IUserRepository userRepository, ITokenService tokenService)
    {
        _logger = logger;
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(FirstLoginUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling first login for user with email: {Email}", request.Email);

        var user = await _userRepository.GetUserAsync(request.Email)
            ?? throw new BadRequestException("User not found.");

        if(user.IsEmailConfirmed == true)
            throw new FirstLoginException("Email confirmed, you should login normally.");

        using (var hmac = new HMACSHA512(user.PasswordSalt))
        {
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.OldPassword));
            if (!computeHash.SequenceEqual(user.PasswordHash) || request.Token != user.ConfirmationToken)
                throw new BadRequestException("Invalid login or password");
        }

        if(string.IsNullOrEmpty(request.NewPassword) || request.NewPassword != request.ConfirmPassword)
            throw new BadRequestException("New password and confirm password do not match.");

        using (var hmac = new HMACSHA512())
        {
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.NewPassword));
        }

        user.IsEmailConfirmed = true;
        user.ConfirmationToken = null;
        user.ConfirmationTokenExpiration = null;

        await _userRepository.AddRolesForUserAsync(request.Email, new List<string> { Roles.Mid.ToString() });

        var token = _tokenService.GetToken(user, false);
        return token;


    }
}
