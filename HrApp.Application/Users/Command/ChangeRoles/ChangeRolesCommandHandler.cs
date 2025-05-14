using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Users.Command.ChangeRoles;

public class ChangeRolesCommandHandler(ILogger<ChangeRolesCommandHandler> logger,
    IUserContext userContext,
    IUserAuthorizationService userAuthorizationService,
    IUserRepository userRepository) : IRequestHandler<ChangeRolesCommand>
{
    private readonly ILogger<ChangeRolesCommandHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;
    private readonly IUserAuthorizationService _userAuthorizationService = userAuthorizationService;
    private readonly IUserRepository _userRepository = userRepository;
    public async Task Handle(ChangeRolesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling ChangeRolesCommand");

        if(!await _userRepository.IfUserExist(request.Email))
        {
            _logger.LogWarning("User does not exist");
            throw new BadRequestException($"User does not exist");
        }

        var currentUser = _userContext.GetCurrentUser();

        if (currentUser == null)
        {
            _logger.LogWarning("Current user is null");
            throw new UnauthorizedException("User is not authorized");
        }

        if (!_userAuthorizationService.Authorize(ResourceOperation.Update))
        {
            throw new UnauthorizedException("User is not authorized to change roles");
        }

        if(request.SelectedRoles.Count == 0)
            request.SelectedRoles.Add("User");

        await _userRepository.AddRolesForUser(request.Email,request.SelectedRoles);
    }
}
