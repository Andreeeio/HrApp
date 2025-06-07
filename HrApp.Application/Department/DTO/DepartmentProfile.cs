using AutoMapper;
using HrApp.Application.Department.Command.AddDepartment;

namespace HrApp.Application.Department.DTO;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<DepartmentDTO, Domain.Entities.Department>();

        CreateMap<Domain.Entities.Department, DepartmentDTO>();

        CreateMap<AddDepartmentCommand, Domain.Entities.Department>();
    }
}
