using HrApp.Application.Interfaces;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Authorizations.Command.CreateNewCode;

public class CreateNewCodeCommandHandler: IRequestHandler<CreateNewCodeCommand>
{
    private readonly ILogger<CreateNewCodeCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IAuthorizationRepository _authorizationRepository;
    private readonly IEmailSender _emailSender;

    public CreateNewCodeCommandHandler(ILogger<CreateNewCodeCommandHandler> logger,
        IUserContext userContext,
        IAuthorizationRepository authorizationRepository,
        IEmailSender emailSender)
    {
        _logger = logger;
        _userContext = userContext;
        _authorizationRepository = authorizationRepository;
        _emailSender = emailSender;
    }

    public async Task Handle(CreateNewCodeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new 2FA code for user");
        var user = _userContext.GetCurrentUser();
        if (user == null)
            throw new UnauthorizedException("User is not authenticated");
        
        var userAuth = await _authorizationRepository.GetUserAuthorizationAsync(Guid.Parse(user.id));
        if (userAuth == null)
            throw new UnauthorizedException("2FA not found for user");

        var random = new Random();
        var verfCod = random.Next(100000, 999999);

        userAuth.AttemptCount = 0;
        userAuth.VerificationCode = verfCod;
        userAuth.VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(15);

        await _authorizationRepository.SaveChangesAsync();

        _logger.LogInformation("Verf code is {verfCode}", verfCod);
        await _emailSender.SendEmailAsync(user.email, "2FA Code", $"Your 2FA code is {verfCod}");
    }
}
