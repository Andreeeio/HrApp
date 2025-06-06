using AutoMapper;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Teams.Command.AddTeam;

public class AddTeamCommandHandler : IRequestHandler<AddTeamCommand>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AddTeamCommandHandler> _logger;
    private readonly IMapper _mapper;

    public AddTeamCommandHandler(ITeamRepository teamRepository, IUserRepository userRepository, ILogger<AddTeamCommandHandler> logger, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Handle(AddTeamCommand request, CancellationToken cancellationToken)
    {
        var team = _mapper.Map<Team>(request);

        var user = await _userRepository.GetUserAsync(request.TeamLeaderEmail);

        if(user == null)
            throw new BadRequestException($"User with email {request.TeamLeaderEmail} does not exist.");
        
        if(user.IsEmailConfirmed == false)
            throw new BadRequestException($"User with email {request.TeamLeaderEmail} has not confirmed their email.");

        team.TeamLeaderId = user.Id;

        await _teamRepository.CreateTeamAsync(team);
        _logger.LogInformation($"Added team {team.Name} to department {team.DepartmentId}");
    }
}