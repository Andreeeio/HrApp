using HrApp.Application.Interfaces;
using HrApp.Application.Services;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Authorizations.Command.Validate2FA;

public class Validate2FARequestHandler(ILogger<Validate2FARequestHandler> logger,
    IUserContext userContext,
    IAuthorizationRepository authorizationRepository,
    IIpAddressService ipAddressService,
    IUserIpAddressRepository userIpAddressRepository,
    ITokenService tokenService,
    IEmailSender emailSender) : IRequestHandler<Validate2FARequest,string>
{
    private readonly ILogger<Validate2FARequestHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;
    private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;
    private readonly IIpAddressService _ipAddressService = ipAddressService;
    private readonly IUserIpAddressRepository _userIpAddressRepository = userIpAddressRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IEmailSender _emailSender = emailSender;
    public async Task<string> Handle(Validate2FARequest request, CancellationToken cancellationToken)
    {
        var user = _userContext.GetCurrentUser();

        var allowedRoles = new[] { Roles.TeamLeader.ToString(), Roles.Hr.ToString(), Roles.Ceo.ToString() };

        if (user == null || !allowedRoles.Any(role => user.IsInRole(role)))
            throw new UnauthorizedAccessException("User is not allowed to perform 2FA");

        bool ipVerification = false;
        var ipAddress = _ipAddressService.GetUserIpAddress();
        var userAgent = _ipAddressService.GetUserAgent();

        var userIps = await _userIpAddressRepository.GetUserIpAddressesAsync(Guid.Parse(user.id));

        if (userIps.Count > 0)
        {
            var uIp = userIps.FirstOrDefault(x => x.IpAddress == ipAddress && x.UserAgent == userAgent);

            if (uIp == null)
                throw new UnauthorizedException("User IP address is not registered in the database");
            else
            {
                uIp.LastAccessed = DateTime.UtcNow;
                await _userIpAddressRepository.SaveChangesAsync();
                ipVerification = true;
            }
        }

        var userAuth = await _authorizationRepository.GetUserAuthorizationAsync(Guid.Parse(user.id));

        if(userAuth == null || !userAuth.IsUsed)
        {
            if (userAuth != null)
                await _authorizationRepository.RemoveAuthorization(userAuth);

            throw new No2FAException("User need to have 2FA authorization");
        }
        else
        {
            if(userAuth.VerificationCodeExpiration > DateTime.UtcNow)
            {
                if(userAuth.VerificationCode == request.Code)
                {
                    var token = _tokenService.GetToken(user, ipVerification);
                    userAuth.AttemptCount = 0;

                    await _authorizationRepository.SaveChangesAsync();

                    return token;
                }
                else
                {
                    userAuth.AttemptCount += 1;

                    await _authorizationRepository.SaveChangesAsync();

                }
            }
            else
            {
                var random = new Random();
                var verfCod = random.Next(100000, 999999);

                userAuth.AttemptCount = 1;
                userAuth.VerificationCode = verfCod;
                userAuth.VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(15);

                await _authorizationRepository.SaveChangesAsync();

                _logger.LogInformation("Verf code is {verfCode}", verfCod);
                await _emailSender.SendEmailAsync(user.email, "2FA Code", $"Your 2FA code is {verfCod}");
            }
        }

        throw new BadRequestException("Invalid veryfication code");
    }
}
