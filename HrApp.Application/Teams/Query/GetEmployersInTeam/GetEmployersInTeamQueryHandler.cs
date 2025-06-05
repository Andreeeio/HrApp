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
    IUserRepository userRepository,
    IMapper mapper) : IRequestHandler<GetEmployersInTeamQuery, List<UserDTO>>
{
    private readonly ILogger<GetEmployersInTeamQueryHandler> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<List<UserDTO>> Handle(GetEmployersInTeamQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all employers in team {TeamId}", request.TeamId);

        var users = await _userRepository.GetUserInTeamAsync(request.TeamId);
        var usersDTO = _mapper.Map<List<UserDTO>>(users);

        return usersDTO;
    }
}
