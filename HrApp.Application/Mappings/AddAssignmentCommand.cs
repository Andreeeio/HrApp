using AutoMapper;

namespace HrApp.Application.Mappings;

public class AddAssignmentCommand : Profile
{
    public AddAssignmentCommand()
    {
        CreateMap<AddAssignmentCommand,  Domain.Entities.Assignment> ();
    }
}
