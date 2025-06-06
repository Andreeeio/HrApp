using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.WorkLog.Command.AddWorkLog;

public class AddWorkLogCommandHandler : IRequestHandler<AddWorkLogCommand>
{
    private readonly ILogger<AddWorkLogCommandHandler> _logger;
    private readonly IWorkLogRepository _workLogRepository;

    public AddWorkLogCommandHandler(ILogger<AddWorkLogCommandHandler> logger, IWorkLogRepository workLogRepository)
    {
        _logger = logger;
        _workLogRepository = workLogRepository;
    }
    public async Task Handle(AddWorkLogCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding a new WorkLog for UserId: {UserId} at {StartTime}", request.UserId, request.StartTime);

        var workLog = new Domain.Entities.WorkLog
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            StartTime = request.StartTime,
            EndTime = null, 
            Hours = 0 
        };

        await _workLogRepository.AddWorkLogAsync(workLog);

        _logger.LogInformation("Successfully added WorkLog with Id: {WorkLogId}", workLog.Id);
    }
}