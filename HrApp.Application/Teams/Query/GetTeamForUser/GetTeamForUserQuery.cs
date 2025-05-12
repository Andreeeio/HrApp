using HrApp.Application.Teams.DTO;
using MediatR;

namespace HrApp.Application.Teams.Query.GetTeamForUser;

public class GetTeamForUserQuery(Guid userId) : IRequest<TeamDTO>
{
    public Guid UserId { get; set; } = userId;
}