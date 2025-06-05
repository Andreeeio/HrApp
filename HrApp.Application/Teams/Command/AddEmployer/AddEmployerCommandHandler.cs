using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Teams.Command.AddEmployer;

public class AddEmployerCommandHandler : IRequestHandler<AddEmployerCommand, Guid>
{
    IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ILogger<AddEmployerCommandHandler> _logger;

    public AddEmployerCommandHandler(ILogger<AddEmployerCommandHandler> logger, IUserRepository userRepository, ITeamRepository teamRepository)
    {
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(AddEmployerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding employer {UserEmail} to team {TeamId}",request.UserEmail, request.TeamId);

        var user = await _userRepository.GetUserByEmail(request.UserEmail);
        if (user == null)
        {
            throw new BadRequestException($"User with email {request.UserEmail} not found");
        }

        await _teamRepository.AddEmployer(request.TeamId, user.Id);
        return user.Id;
    }
}
