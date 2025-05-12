using AutoMapper;

namespace HrApp.Application.Mappings;

public class AddTeamCommand : Profile
{
    public AddTeamCommand()
    {
        CreateMap<AddTeamCommand, Domain.Entities.Team>();
    }
}
