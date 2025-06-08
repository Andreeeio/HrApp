using HrApp.Application.Teams.DTO;
using MediatR;

namespace HrApp.Application.Teams.Query.GetTeamForUser;

public class GetTeamForUserQuery : IRequest<TeamDTO>
{
    public Guid? Id { get; set; } = null;

    public GetTeamForUserQuery()
    {
    }
    public GetTeamForUserQuery(Guid id)
    {
        Id = id;
    }
}