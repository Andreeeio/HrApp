using HrApp.Application.Interfaces;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Authorizations.Command.Add2FA;

public class Add2FARequestHandler(ILogger<Add2FARequestHandler> logger,
    IUserContext userContext,
    IAuthorizationRepository authorizationRepository,
    IEmailSender emailSender) : IRequestHandler<Add2FARequest>
{
    private readonly ILogger<Add2FARequestHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;
    private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    public async Task Handle(Add2FARequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating 2FA for user");

        var user = _userContext.GetCurrentUser();
        if (user == null)
        {
            throw new UnauthorizedException("User not found");
        }

        if(request.Email != user.email)
        {
            throw new BadRequestException("Email is not valid");
        }

        var random = new Random();
        int verfCod = random.Next(100000, 999999);

        Authorization authorization = new Authorization
        {
            UserId = Guid.Parse(user.id),
            VerificationCode = verfCod,
            VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(15),
            IsUsed = true,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            AttemptCount = 0
        };
        
        await _authorizationRepository.AddAuthorization(authorization);

        _logger.LogInformation("Verf code is {verfCode}", verfCod);
        await _emailSender.SendEmailAsync(user.email, "2FA Code", $"Your 2FA code is {verfCod}");
    }
}
