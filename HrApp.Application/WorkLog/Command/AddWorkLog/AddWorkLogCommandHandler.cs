using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.WorkLog.Command.AddWorkLog
{
    public class AddWorkLogCommandHandler : IRequestHandler<AddWorkLogCommand>
    {
        private readonly ILogger<AddWorkLogCommandHandler> _logger;
        private readonly IWorkLogRepository _repository;
        public AddWorkLogCommandHandler(ILogger<AddWorkLogCommandHandler> logger, IWorkLogRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task Handle(AddWorkLogCommand request, CancellationToken cancellationToken)
        {
            // Log the start of the operation
            _logger.LogInformation("Adding a new WorkLog for UserId: {UserId} at {StartTime}", request.UserId, request.StartTime);

            // Create a new WorkLog instance
            var workLog = new HrApp.Domain.Entities.WorkLog
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                StartTime = request.StartTime,
                EndTime = null, // WorkLog starts without an end time
                Hours = 0 // Hours will be calculated when the work ends
            };

            // Add the WorkLog to the repository
            await _repository.AddWorkLog(workLog);

            // Log the successful addition
            _logger.LogInformation("Successfully added WorkLog with Id: {WorkLogId}", workLog.Id);

            return;
        }
    }
}
