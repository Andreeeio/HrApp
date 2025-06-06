using MediatR;

namespace HrApp.Application.Assignment.Command.AddAssignmentToTeam;

public class AddAssignmentToTeamCommand : IRequest
{
    public Guid AssignmentId { get; set; }
    public Guid TeamId { get; set; }
}
