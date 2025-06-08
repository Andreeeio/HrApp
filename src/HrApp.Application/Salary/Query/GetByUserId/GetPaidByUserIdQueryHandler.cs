using HrApp.Application.Salary.Query.GetById;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Salary.Query.GetByUserId;

public class GetPaidByUserIdQueryHandler : IRequestHandler<GetPaidByUserIdQuery, Paid>
{
    private readonly ISalaryRepository _repository;
    private readonly ILogger<GetPaidByIdQueryHandler> _logger;
    public GetPaidByUserIdQueryHandler(ILogger<GetPaidByIdQueryHandler> logger, ISalaryRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    public async Task<Paid> Handle(GetPaidByUserIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting paid entry with ID: {Id}", request.Id);
        var paid = await _repository.GetPaidByUserIdAsync(request.Id);
        if (paid == null)
        {
            _logger.LogWarning("Paid with ID {Id} not found", request.Id);
            throw new BadRequestException("Paid with not found.");
        }
        return paid;
    }
}
