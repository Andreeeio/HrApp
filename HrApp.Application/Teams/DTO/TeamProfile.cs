using AutoMapper;
using HrApp.Domain.Entities;

namespace HrApp.Application.Teams.DTO;

public class TeamProfile : Profile
{
    public TeamProfile()
    {
        CreateMap<Team, TeamDTO>();
        CreateMap<TeamDTO, Team>();
    }
}
