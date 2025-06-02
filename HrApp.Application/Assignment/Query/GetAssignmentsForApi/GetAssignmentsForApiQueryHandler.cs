using AutoMapper;
using HrApp.Application.Assignment.DTO;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Assignment.Query.GetAssignmentsForApi;

public class GetAssignmentsForApiQueryHandler(ILogger<GetAssignmentsForApiQueryHandler> logger,
    IAssignmentRepository assignmentRepository,
    IMapper mapper) : IRequestHandler<GetAssignmentsForApiQuery, List<AssignmentApiDTO>>
{
    private readonly ILogger<GetAssignmentsForApiQueryHandler> _logger = logger;
    private readonly IAssignmentRepository _assignmentRepository = assignmentRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<List<AssignmentApiDTO>> Handle(GetAssignmentsForApiQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAssignmentsForApiQuery for api request");
         
        var assignments = await _assignmentRepository.GetAssignments(
            request.Name,
            request.IsEnded,
            request.AssignedToTeamId,
            request.DifficultyLevel,
            cancellationToken);

        var assignmentsDto = _mapper.Map<List<AssignmentApiDTO>>(assignments);

        return assignmentsDto;
    }
}
