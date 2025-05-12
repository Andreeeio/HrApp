using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Assignment.DTO
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<Domain.Entities.Assignment, AssignmentDTO>();
            CreateMap<AssignmentDTO, Domain.Entities.Assignment>();
        }
    }
}
