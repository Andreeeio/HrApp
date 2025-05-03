using HrApp.Application.Team.DTO;
using MediatR;

namespace HrApp.Application.Team.Query.GetAllTeams;

public class GetAllTeamsQuery : IRequest<List<TeamDTO>>
{
}
