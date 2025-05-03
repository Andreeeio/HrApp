using AutoMapper;
using HrApp.Application.Department.DTO;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Department.Query.GetAllDepartments
{
    public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, List<DepartmentDTO>>
    {
        private readonly ILogger<GetAllDepartmentsQueryHandler> _logger;
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;
        public GetAllDepartmentsQueryHandler(ILogger<GetAllDepartmentsQueryHandler> logger,IDepartmentRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<DepartmentDTO>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all departments");
            var departments = await _repository.GetAllDepartments();
            var dto = _mapper.Map<List<DepartmentDTO>>(departments);

            return dto;
        }
    }
}
