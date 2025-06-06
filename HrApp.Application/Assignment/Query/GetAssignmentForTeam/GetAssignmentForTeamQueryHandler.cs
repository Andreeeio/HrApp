using AutoMapper;
using HrApp.Application.Assignment.DTO;
using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Assignment.Query.GetAssignmentForTeam;

public class GetAssignmentForTeamQueryHandler : IRequestHandler<GetAssignmentForTeamQuery, List<AssignmentDTO>>
{
    private readonly ILogger<GetAssignmentForTeamQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IAssignmentRepository _repository;
    private readonly IMapper _mapper;

    public GetAssignmentForTeamQueryHandler(ILogger<GetAssignmentForTeamQueryHandler> logger,
        IUserContext userContext,
        IAssignmentRepository repository,
        IMapper mapper)
    {
        _logger = logger;
        _userContext = userContext;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<AssignmentDTO>> Handle(GetAssignmentForTeamQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _userContext.GetCurrentUser();
        _logger.LogInformation("Getting all assignments for team {TeamId}", request.AssignedToTeamId);

        if (currentUser == null)
            throw new UnauthorizedException("User is not authenticated");

        var assignments = await _repository.GetAllAssignmentsForTeamAsync(request.AssignedToTeamId);

        var dto = _mapper.Map<List<AssignmentDTO>>(assignments);

        return dto;
    }
}