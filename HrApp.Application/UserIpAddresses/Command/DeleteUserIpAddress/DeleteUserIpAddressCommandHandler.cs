using HrApp.Application.Interfaces;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.UserIpAddresses.Command.DeleteUserIpAddress;

public class DeleteUserIpAddressCommandHandler : IRequestHandler<DeleteUserIpAddressCommand,string>
{
    private readonly ILogger<DeleteUserIpAddressCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly ITokenService _tokenService;
    private readonly IUserIpAddressRepository _userIpAddressRepository;

    public DeleteUserIpAddressCommandHandler(ILogger<DeleteUserIpAddressCommandHandler> logger,
        IUserContext userContext, 
        IUserIpAddressRepository userIpAddressRepository, 
        ITokenService tokenService)
    {
        _logger = logger;
        _userContext = userContext;
        _userIpAddressRepository = userIpAddressRepository;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(DeleteUserIpAddressCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteUserIpAddressCommand");
        var user = _userContext.GetCurrentUser();

        if (user == null)
            throw new UnauthorizedException("User is not authenticated");

        await _userIpAddressRepository.DeleteUserIpAddressAsync(Guid.Parse(user.id));
        return _tokenService.GetToken(user, false);
    }
}
