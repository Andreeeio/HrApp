using AutoMapper;
using HrApp.Application.Assignment.DTO;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Assignment.Query.GetAssignmentsForApi;

public class GetAssignmentsForApiQueryHandler(ILogger<GetAssignmentsForApiQueryHandler> logger,
    IAssignmentRepository assignmentRepository,
    IApiLogRepository apiLogRepository,
    IMapper mapper) : IRequestHandler<GetAssignmentsForApiQuery, List<AssignmentApiDTO>>
{
    private readonly ILogger<GetAssignmentsForApiQueryHandler> _logger = logger;
    private readonly IAssignmentRepository _assignmentRepository = assignmentRepository;
    private readonly IApiLogRepository _apiLogRepository = apiLogRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<List<AssignmentApiDTO>> Handle(GetAssignmentsForApiQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAssignmentsForApiQuery for api request");

        ApiLog log = new()
        {
            Name = request.Name ?? "",
            IsEnded = request.IsEnded,
            DifficultyLevel = request.DifficultyLevel,
            AssignedToTeamId = request.AssignedToTeamId,
            CreatedAt = DateTime.UtcNow
        };

        await _apiLogRepository.AddLogAsync(log);

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
