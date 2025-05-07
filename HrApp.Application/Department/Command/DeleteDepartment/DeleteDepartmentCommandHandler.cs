using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Department.Command.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
    {
        private readonly IDepartmentRepository _repository;
        private readonly ILogger<DeleteDepartmentCommandHandler> _logger;
        public DeleteDepartmentCommandHandler(ILogger<DeleteDepartmentCommandHandler> logger, IDepartmentRepository repository)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting department {DepartmentId}", request.DepartmentId);
            await _repository.DeleteDepartment(request.DepartmentId);
            return;
        }
    }
}
