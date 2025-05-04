using AutoMapper;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Department.Command.AddDepartment
{
    public class AddDepartmentCommandHandler : IRequestHandler<AddDepartmentCommand>
    {
        private readonly ILogger _logger;
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;

        public AddDepartmentCommandHandler(ILogger<AddDepartmentCommandHandler> logger, IDepartmentRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = _mapper.Map<Domain.Entities.Department>(request);
            _logger.LogInformation("Adding department {DepartmentName}", department.Name);
            await _repository.CreateDepartment(department);

            return;
        }
    }
}
