using AutoMapper;
using HrApp.Application.Assignment.Command.AddAssignment;
using HrApp.Application.Assignment.Command.EditAssignment;
using HrApp.Domain.Entities;

namespace HrApp.Application.Assignment.DTO;

public class AssignmentProfile : Profile
{
    public AssignmentProfile()
    {
        CreateMap<Domain.Entities.Assignment, AssignmentDTO>();

        CreateMap<AssignmentDTO, Domain.Entities.Assignment>();
        
        CreateMap<Domain.Entities.Assignment, AssignmentApiDTO>();
        
        CreateMap<Domain.Entities.Assignment, EditAssignmentCommand>();

        CreateMap<AddAssignmentCommand, Domain.Entities.Assignment>();

        CreateMap<Domain.Entities.Assignment, AssignmentRaport>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.AssignmentId, opt => opt.MapFrom(a => a.Id));
    }
}
