using HrApp.Application.Interfaces;
using HrApp.Application.Users.DTO;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.UserIpAddresses.Command.AddUserIpAddress;

public class AddUserIpAddressCommandHandler : IRequestHandler<AddUserIpAddressCommand, string>
{
    private readonly ILogger<AddUserIpAddressCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IIpAddressService _ipAddressService;
    private readonly IUserIpAddressRepository _userIpAddressRepository;
    private readonly ITokenService _tokenService;

    public AddUserIpAddressCommandHandler(ILogger<AddUserIpAddressCommandHandler> logger,
        IUserContext userContext,
        IIpAddressService ipAddressService,
        ITokenService tokenService,
        IUserIpAddressRepository userIpAddressRepository)
    {
        _logger = logger;
        _userContext = userContext;
        _ipAddressService = ipAddressService;
        _tokenService = tokenService;
        _userIpAddressRepository = userIpAddressRepository;
    }

    public async Task<string> Handle(AddUserIpAddressCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling AddUserIpAddressCommand");
        var user = _userContext.GetCurrentUser();

        if (user == null)
            throw new UnauthorizedException("User is not authenticated");

        var ipAddress = _ipAddressService.GetUserIpAddress();
        var userAgent = _ipAddressService.GetUserAgent();

        if (await _userIpAddressRepository.IfUserIpAddressExistAsync(Guid.Parse(user.id), ipAddress))
            throw new BadRequestException("User IP address already exists in the database");

        var entity = new UserIpAddress
        {
            UserId = Guid.Parse(user.id),
            IpAddress = ipAddress,
            UserAgent = userAgent,
            LastAccessed = DateTime.UtcNow,
            IsActive = true
        };

        await _userIpAddressRepository.AddUserIpAddressAsync(entity);
        return _tokenService.GetToken(user, true);
    }
}
