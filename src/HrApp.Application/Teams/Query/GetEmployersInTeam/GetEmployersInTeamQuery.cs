using HrApp.Application.Users.DTO;
using MediatR;

namespace HrApp.Application.Teams.Query.GetEmployersInTeam;

public class GetEmployersInTeamQuery(Guid teamId) : IRequest<List<UserDTO>>
{
    public Guid TeamId { get; set; } = teamId;
}
