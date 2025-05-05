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
        private readonly IDepartmentRepository _repositoryd;
        private readonly IUserRepository _repositoryu;
        private readonly IMapper _mapper;

        public AddDepartmentCommandHandler(ILogger<AddDepartmentCommandHandler> logger, IDepartmentRepository repositoryd,IUserRepository repositoryu, IMapper mapper)
        {
            _logger = logger;
            _repositoryd = repositoryd;
            _mapper = mapper;
            _repositoryu = repositoryu;
        }
        public async Task Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = _mapper.Map<Domain.Entities.Department>(request);
            department.HeadOfDepartment = await _repositoryu.GetUserById(request.HeadOfDepartmentId);
            _logger.LogInformation("Adding department {DepartmentName}", department.Name);
            await _repositoryd.CreateDepartment(department);

            return;
        }
    }
}
