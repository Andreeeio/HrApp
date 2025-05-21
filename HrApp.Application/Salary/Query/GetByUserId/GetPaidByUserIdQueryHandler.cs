using HrApp.Application.Salary.Query.GetById;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Salary.Query.GetByUserId
{
    public class GetPaidByUserIdQueryHandler : IRequestHandler<GetPaidByUserIdQuery, Domain.Entities.Paid>
    {
        private readonly ISalaryRepository _repository;
        private readonly ILogger<GetPaidByIdQueryHandler> _logger;
        public GetPaidByUserIdQueryHandler(ILogger<GetPaidByIdQueryHandler> logger, ISalaryRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task<Domain.Entities.Paid> Handle(GetByUserId.GetPaidByUserIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting paid entry with ID: {Id}", request.Id);
            var paid = await _repository.GetPaidByUserId(request.Id);
            if (paid == null)
            {
                _logger.LogWarning("Paid with ID {Id} not found", request.Id);
                throw new KeyNotFoundException($"Paid with ID {request.Id} not found.");
            }
            return paid;
        }
    }
}
