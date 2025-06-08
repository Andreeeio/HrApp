using MediatR;

namespace HrApp.Application.Teams.Command.DeleteTeam;

public class DeleteTeamCommand : IRequest
{
    public Guid TeamId { get; set; }
}