using AutoMapper;
using HrApp.Application.Users.DTO;
using HrApp.Domain.Constants;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Users.Query.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
{
    private readonly ILogger<GetAllUsersQueryHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly ITeamAuthorizationService _teamAuthorizationService;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(
        ILogger<GetAllUsersQueryHandler> logger,
        IUserRepository userRepository,
        ITeamAuthorizationService teamAuthorizationService,
        IMapper mapper)
    {
        _logger = logger;
        _userRepository = userRepository;
        _teamAuthorizationService = teamAuthorizationService;
        _mapper = mapper;
    }

    public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllUsersQuery");

        if(!_teamAuthorizationService.Authorize(ResourceOperation.Read))
        {
            _logger.LogWarning("Unauthorized access attempt to GetAllUsersQuery");
            throw new UnauthorizedAccessException("You do not have permission to access this resource.");
        }

        var users = await _userRepository.GetAllUsersAsync();

        var dto = _mapper.Map<List<UserDTO>>(users);

        return dto;
    }
}
