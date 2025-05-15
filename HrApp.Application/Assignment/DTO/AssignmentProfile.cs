using AutoMapper;
using HrApp.Application.Assignment.Command.EditAssignment;

namespace HrApp.Application.Assignment.DTO;

public class AssignmentProfile : Profile
{
    public AssignmentProfile()
    {
        CreateMap<Domain.Entities.Assignment, AssignmentDTO>();
        CreateMap<AssignmentDTO, Domain.Entities.Assignment>();
        CreateMap<Domain.Entities.Assignment, EditAssignmentCommand>();
    }
}
