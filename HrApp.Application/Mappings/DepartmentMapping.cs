using AutoMapper;
using HrApp.Application.Department.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Mappings
{
    public class DepartmentMapping : Profile
    {
        public DepartmentMapping()
        {
            CreateMap<DepartmentDTO, Domain.Entities.Department>();


            CreateMap<Domain.Entities.Department, DepartmentDTO>();
        }
    }
}
