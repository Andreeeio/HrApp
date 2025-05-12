using AutoMapper;
using HrApp.Application.WorkLog.DTO;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.WorkLog.Query.GetWorkLog;

public class GetWorkLogQueryHandler : IRequestHandler<GetWorkLogQuery, List<WorkLogDTO>>
{
    private readonly ILogger<GetWorkLogQueryHandler> _logger;
    private readonly IWorkLogRepository _repository;
    private readonly IMapper _mapper;
    public GetWorkLogQueryHandler(ILogger<GetWorkLogQueryHandler> logger, IWorkLogRepository repository, IMapper mapper) 
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<List<WorkLogDTO>> Handle(GetWorkLogQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetWorkLogQuery for user {UserId}", request.UserId);
        var workLogs = await _repository.GetWorkLogsByUserId(request.UserId);
        var dto = _mapper.Map<List<WorkLogDTO>>(workLogs);
        return dto;
    }
}