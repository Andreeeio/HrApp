using AutoMapper;
using HrApp.Application.Users.DTO;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Teams.Query.GetEmployersInTeam;

public class GetEmployersInTeamQueryHandler(ILogger<GetEmployersInTeamQueryHandler> logger,
    ITeamAuthorizationService teamAuthorizationService,
    IUserRepository userRepository,
    IMapper mapper) : IRequestHandler<GetEmployersInTeamQuery, List<UserDTO>>
{
    private readonly ILogger<GetEmployersInTeamQueryHandler> _logger = logger;
    private readonly ITeamAuthorizationService _teamAuthorizationService = teamAuthorizationService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<List<UserDTO>> Handle(GetEmployersInTeamQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all employers in team {TeamId}", request.TeamId);

        //if (!_teamAuthorizationService.Authorize(ResourceOperation.Read))
        //    throw new AccessForbiddenException("User is not authorized");

        var users = await _userRepository.GetUserInTeamAsync(request.TeamId);
        var usersDTO = _mapper.Map<List<UserDTO>>(users);

        return usersDTO;
    }
}
