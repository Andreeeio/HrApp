using HrApp.Application.Teams.DTO;
using MediatR;

namespace HrApp.Application.Teams.Query.GetAllTeams;

public class GetAllTeamsQuery : IRequest<List<TeamDTO>>
{
}
