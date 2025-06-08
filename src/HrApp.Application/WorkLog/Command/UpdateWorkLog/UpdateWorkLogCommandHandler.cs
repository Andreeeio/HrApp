using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.WorkLog.Command.UpdateWorkLog;

public class UpdateWorkLogCommandHandler : IRequestHandler<UpdateWorkLogCommand>
{
    private readonly ILogger<UpdateWorkLogCommandHandler> _logger;
    private readonly IWorkLogRepository _repository;
    public UpdateWorkLogCommandHandler(ILogger<UpdateWorkLogCommandHandler> logger, IWorkLogRepository repository) 
    {
        _logger = logger;
        _repository = repository;
    }
    public async Task Handle(UpdateWorkLogCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating WorkLog with Id: {WorkLogId}", request.Id);

        var workLog = await _repository.GetWorkLogByIdAsync(request.Id);

        if (workLog == null)
        {
            _logger.LogWarning("WorkLog with Id: {WorkLogId} not found", request.Id);
            throw new BadRequestException($"WorkLog with Id {request.Id} not found.");
        }

        workLog.EndTime = request.EndTime;
        workLog.Hours = request.Hours;

        await _repository.UpdateWorkLogAsync(workLog);

        _logger.LogInformation("Successfully updated WorkLog with Id: {WorkLogId}", request.Id);
    }
}