using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Salary.Query.GetById;

public class GetPaidByIdQueryHandler : IRequestHandler<GetPaidByIdQuery, Paid>
{
    private readonly ISalaryRepository _salaryRepository;
    private readonly ILogger<GetPaidByIdQueryHandler> _logger;
    public GetPaidByIdQueryHandler(ILogger<GetPaidByIdQueryHandler> logger, ISalaryRepository salaryRepository)
    {
        _logger = logger;
        _salaryRepository = salaryRepository;
    }
    public async Task<Domain.Entities.Paid> Handle(GetPaidByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting paid entry with ID: {Id}", request.Id);
        var paid = await _salaryRepository.GetPaidByIdAsync(request.Id);
        if (paid == null)
        {
            _logger.LogWarning("Paid with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Paid with ID {request.Id} not found.");
        }
        return paid;
    }
}
