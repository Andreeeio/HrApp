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

namespace HrApp.Application.Salary.Command.AddPaid
{
    public class AddPaidCommandHandler : IRequestHandler<AddPaidCommand>
    {
        private readonly ISalaryRepository _repository;
        private readonly ILogger<AddPaidCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddPaidCommandHandler(ILogger<AddPaidCommandHandler> logger ,ISalaryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Handle(AddPaidCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding new paid record for user with ID: {UserId}", request.UserId);
            var paid = _mapper.Map<Paid>(request);
            await _repository.AddPaid(paid);
            return;
        }
    }
}
