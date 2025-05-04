using AutoMapper;
using HrApp.Application.Interfaces;
using HrApp.Application.Teams.DTO;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Teams.Query.GetTeamForDepartment;

public class GetTeamForDepartmentQueryHandler(ILogger<GetTeamForDepartmentQueryHandler> logger,
    IUserContext userContext,
    ITeamRepository teamRepository,
    ITeamAuthorizationService teamAuthorizationService,
    IMapper mapper) : IRequestHandler<GetTeamForDepartmentQuery, List<TeamDTO>>
{
    private readonly ILogger<GetTeamForDepartmentQueryHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;
    private readonly ITeamRepository _teamRepository = teamRepository;
    private readonly ITeamAuthorizationService _teamAuthorizationService = teamAuthorizationService;
    private readonly IMapper _mapper = mapper;
    public async Task<List<TeamDTO>> Handle(GetTeamForDepartmentQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _userContext.GetCurrentUser();
        _logger.LogInformation("Getting all teams for department {DepartmentId}", request.DepartmentId);

        if (currentUser == null)
            throw new UnauthorizedException("User is not authenticated");

        if (!_teamAuthorizationService.Authorize(ResourceOperation.Read))
            throw new AccessForbiddenException("User is not authorized");
        
        var teams = await _teamRepository.GetAllTeamsForDepartment(request.DepartmentId);

        var teamsDTO = _mapper.Map<List<TeamDTO>>(teams);

        return teamsDTO;
    }
}
