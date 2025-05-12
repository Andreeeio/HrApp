using AutoMapper;
using HrApp.Application.Assignment.DTO;
using HrApp.Application.Interfaces;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Assignment.Query.GetAssignmentForTeam;

public class GetAssignmentForTeamQueryHandler(ILogger<GetAssignmentForTeamQueryHandler> logger,
    IUserContext userContext,
    IAssignmentRepository repository,
    ITeamAuthorizationService teamAuthorizationService,
    IMapper mapper) : IRequestHandler<GetAssignmentForTeamQuery, List<AssignmentDTO>>
{
    private readonly ILogger<GetAssignmentForTeamQueryHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;
    private readonly IAssignmentRepository _repository = repository;
    private readonly ITeamAuthorizationService _teamAuthorizationService = teamAuthorizationService;
    private readonly IMapper _mapper = mapper;
    public async Task<List<AssignmentDTO>> Handle(GetAssignmentForTeamQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _userContext.GetCurrentUser();
        _logger.LogInformation("Getting all assignments for team {TeamId}", request.AssignedToTeamId);

        if (currentUser == null)
            throw new UnauthorizedException("User is not authenticated");

        //if (!_teamAuthorizationService.Authorize(ResourceOperation.Read))
        //    throw new AccessForbiddenException("User is not authorized");

        var assignments = await _repository.GetAllAssignmentsForTeam(request.AssignedToTeamId);

        var dto = _mapper.Map<List<AssignmentDTO>>(assignments);

        return dto;
    }
}