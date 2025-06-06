using AutoMapper;
using HrApp.Application.Assignment.DTO;
using HrApp.Application.Interfaces;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Assignment.Query.GetFreeAssignments;

public class GetFreeAssignmentsQueryHandler : IRequestHandler<GetFreeAssignmentsQuery, List<AssignmentDTO>>
{
    private readonly ILogger<GetFreeAssignmentsQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;

    public GetFreeAssignmentsQueryHandler(ILogger<GetFreeAssignmentsQueryHandler> logger,
     IUserContext userContext,
     IAssignmentRepository repository,
     IMapper mapper)
    {
        _logger = logger;
        _userContext = userContext;
        _assignmentRepository = repository;
        _mapper = mapper;
    }

    public async Task<List<AssignmentDTO>> Handle(GetFreeAssignmentsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _userContext.GetCurrentUser();
        _logger.LogInformation("Getting all active assignments");

        if (currentUser == null)
            throw new UnauthorizedException("User is not authenticated");

        var assignments = await _assignmentRepository.GetAssignmentsAsync(true);

        var dto = _mapper.Map<List<AssignmentDTO>>(assignments);

        return dto;
    }
}
