using AutoMapper;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.WorkLog.Command.AddWorkLogExportHistory;

public class AddWorkLogExportHistoryCommandHandler : IRequestHandler<AddWorkLogExportHistoryCommand>
{
    private readonly IWorkLogExportHistoryRepository _workLogExportHistoryRepository;
    private readonly ILogger<AddWorkLogExportHistoryCommandHandler> _logger;
    private readonly IMapper _mapper;

    public AddWorkLogExportHistoryCommandHandler(
        IWorkLogExportHistoryRepository workLogExportHistoryRepository,
        ILogger<AddWorkLogExportHistoryCommandHandler> logger,
        IMapper mapper)
    {
        _workLogExportHistoryRepository = workLogExportHistoryRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Handle(AddWorkLogExportHistoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Saving export history for user {ExportedByUserId} (exported for {ExportedForUserId})",
            request.ExportedByUserId, request.ExportedForUserId);
        
        var history = _mapper.Map<WorkLogExportHistory>(request);

        await _workLogExportHistoryRepository.AddAsync(history);

        _logger.LogInformation("Export history saved with ID {HistoryId}", history.Id);
    }
}
