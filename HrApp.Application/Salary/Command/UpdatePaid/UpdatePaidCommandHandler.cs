using AutoMapper;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Salary.Command.UpdatePaid
{
    public class UpdatePaidCommandHandler : IRequestHandler<UpdatePaidCommand>
    {
        private readonly ISalaryRepository _repository;
        private readonly ILogger<UpdatePaidCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdatePaidCommandHandler(ILogger<UpdatePaidCommandHandler> logger, ISalaryRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Handle(UpdatePaidCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating paid entry with ID: {Id}", request.Id);

            var paid = await _repository.GetPaidById(request.Id);
            if (paid == null)
            {
                _logger.LogWarning("Paid with ID {Id} not found", request.Id);
                throw new KeyNotFoundException($"Paid with ID {request.Id} not found.");
            }
            
            _mapper.Map(request, paid); // zaktualizuj encję
            await _repository.UpdatePaid(paid);
            return;
        }
    }
}
