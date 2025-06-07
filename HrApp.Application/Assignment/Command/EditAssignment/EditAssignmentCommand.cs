using MediatR;

namespace HrApp.Application.Assignment.Command.EditAssignment;

public class EditAssignmentCommand : IRequest
{
    public Guid Id { get; set; }
    public DateTime EndDate { get; set; }
    public int DifficultyLevel { get; set; }

    public EditAssignmentCommand() { } 

    public EditAssignmentCommand(Guid id)
    {
        Id = id;
    }
}
