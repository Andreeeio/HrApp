using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.WorkLog.Command.UpdateWorkLog
{
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
            // Log the start of the operation
            _logger.LogInformation("Updating WorkLog with Id: {WorkLogId}", request.Id);

            // Retrieve the existing WorkLog
            var workLog = await _repository.GetWorkLogById(request.Id);

            if (workLog == null)
            {
                _logger.LogWarning("WorkLog with Id: {WorkLogId} not found", request.Id);
                throw new InvalidOperationException($"WorkLog with Id {request.Id} not found.");
            }

            // Update the WorkLog properties
            workLog.EndTime = request.EndTime;
            workLog.Hours = request.Hours;

            // Save the changes
            await _repository.UpdateWorkLog(workLog);

            // Log the successful update
            _logger.LogInformation("Successfully updated WorkLog with Id: {WorkLogId}", request.Id);

            return;
        }
    }
}
