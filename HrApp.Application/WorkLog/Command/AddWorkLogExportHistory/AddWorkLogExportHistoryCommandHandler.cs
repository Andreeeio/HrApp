using AutoMapper;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.WorkLog.Command.AddWorkLogExportHistory
{
    public class AddWorkLogExportHistoryCommandHandler : IRequestHandler<AddWorkLogExportHistoryCommand>
    {
        private readonly IWorkLogExportHistoryRepository _repository;
        private readonly ILogger<AddWorkLogExportHistoryCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddWorkLogExportHistoryCommandHandler(
            IWorkLogExportHistoryRepository repository,
            ILogger<AddWorkLogExportHistoryCommandHandler> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Handle(AddWorkLogExportHistoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Saving export history for user {ExportedByUserId} (exported for {ExportedForUserId})",
                request.ExportedByUserId, request.ExportedForUserId);
            
            var history = _mapper.Map<WorkLogExportHistory>(request);

            await _repository.AddAsync(history);

            _logger.LogInformation("Export history saved with ID {HistoryId}", history.Id);
        }
    }
}
