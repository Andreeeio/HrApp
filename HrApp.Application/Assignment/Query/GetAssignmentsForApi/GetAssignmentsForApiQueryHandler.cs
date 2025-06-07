using AutoMapper;
using HrApp.Application.Assignment.DTO;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Assignment.Query.GetAssignmentsForApi;

public class GetAssignmentsForApiQueryHandler : IRequestHandler<GetAssignmentsForApiQuery, List<AssignmentApiDTO>>
{
    private readonly ILogger<GetAssignmentsForApiQueryHandler> _logger;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IApiLogRepository _apiLogRepository;
    private readonly IMapper _mapper;

    public GetAssignmentsForApiQueryHandler(ILogger<GetAssignmentsForApiQueryHandler> logger,
        IAssignmentRepository assignmentRepository,
        IApiLogRepository apiLogRepository,
        IMapper mapper)
    {
        _logger = logger;
        _assignmentRepository = assignmentRepository;
        _apiLogRepository = apiLogRepository;
        _mapper = mapper;
    }

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

        var assignments = await _assignmentRepository.GetApiAssignmentsAsync(
            request.Name,
            request.IsEnded,
            request.AssignedToTeamId,
            request.DifficultyLevel,
            cancellationToken);

        var assignmentsDto = _mapper.Map<List<AssignmentApiDTO>>(assignments);

        return assignmentsDto;
    }
}
