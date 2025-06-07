using MediatR;

namespace HrApp.Application.Assignment.Command.CompleteAssignment;

public class CompleteAssignmentCommand : IRequest
{
    public CompleteAssignmentCommand(Guid assignmentId)
    {
        AssignmentId = assignmentId;
    }
    public Guid AssignmentId { get; set; }
}
