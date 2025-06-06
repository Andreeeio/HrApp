using AutoMapper;
using HrApp.Application.Teams.Command.AddTeam;
using HrApp.Domain.Entities;

namespace HrApp.Application.Teams.DTO;

public class TeamProfile : Profile
{
    public TeamProfile()
    {
        CreateMap<Team, TeamDTO>();

        CreateMap<AddTeamCommand, Team>();

        CreateMap<TeamDTO, Team>();
        
        CreateMap<Team, TeamRaport>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TeamId, opt => opt.MapFrom(t => t.Id));
    }
}
