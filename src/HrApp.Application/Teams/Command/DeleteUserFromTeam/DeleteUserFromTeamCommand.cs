using MediatR;

namespace HrApp.Application.Teams.Command.DeleteUserFromTeam;

public class DeleteUserFromTeamCommand : IRequest
{
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
}