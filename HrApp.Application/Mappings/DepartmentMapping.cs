using AutoMapper;
using HrApp.Application.Department.DTO;

namespace HrApp.Application.Mappings;

public class DepartmentMapping : Profile
{
    public DepartmentMapping()
    {
        CreateMap<DepartmentDTO, Domain.Entities.Department>();


        CreateMap<Domain.Entities.Department, DepartmentDTO>();
    }
}
