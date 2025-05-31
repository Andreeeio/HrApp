using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace HrApp.Application.Users.Query.LoginUser;

public class LoginUserQueryHandler(ILogger<LoginUserQueryHandler> logger,
    IUserRepository userRepository,
    ITokenService tokenService,
    IIpAddressService ipAddressService,
    IUserIpAddressRepository userIpAddressRepository,
    IAuthorizationRepository authorizationRepository) : IRequestHandler<LoginUserQuery,(string, int)>
{
    private readonly ILogger<LoginUserQueryHandler> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IIpAddressService _ipAddressService = ipAddressService;
    private readonly IUserIpAddressRepository _userIpAddressRepository = userIpAddressRepository;
    private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;
    public async Task<(string,int)> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User tring to log in");

        var user = await _userRepository.GetUserByEmail(request.Email)
            ?? throw new BadRequestException("Invalid login or password");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

        if (!computeHash.SequenceEqual(user.PasswordHash))
            throw new BadRequestException("Invalid login or password");
        
        bool ipVerification = false;
        var ipAddress = _ipAddressService.GetUserIpAddress();
        var userAgent = _ipAddressService.GetUserAgent();

        var userIps = await _userIpAddressRepository.GetUserIpAddressesAsync(user.Id);
        
        if(userIps.Count > 0)
        {
            var uIp = userIps.FirstOrDefault(x => x.IpAddress == ipAddress && x.UserAgent == userAgent);

            if (uIp == null)
                throw new BadRequestException("User IP address is not registered in the database");
            else
            {
                uIp.LastAccessed = DateTime.UtcNow;
                await _userIpAddressRepository.SaveChangesAsync();
                ipVerification = true;
            }
        }

        string token;

        if (user.Roles.Any(x => x.Name == Roles.TeamLeader.ToString())
            || user.Roles.Any(x => x.Name == Roles.Hr.ToString())
            || user.Roles.Any(x => x.Name == Roles.Ceo.ToString()))
        {
            var userAuth = await _authorizationRepository.GetUserAuthorizationAsync(user.Id);
            token = _tokenService.GetToken(user, ipVerification);
            if (userAuth == null || !userAuth.IsActive)
            {
                return (token, 2);
            }
            return (token, 1);
        }

        token = _tokenService.GetToken(user, ipVerification);

        return (token,0);
    }
}
