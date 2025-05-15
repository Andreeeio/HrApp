using HrApp.Application.Assignment.Command.EditAssignment;
using HrApp.Application.Assignment.DTO;
using MediatR;

namespace HrApp.Application.Assignment.Query.GetAssignmentById;

public class GetAssignmentByIdQuery : IRequest<EditAssignmentCommand>
{
    public Guid Id { get; set; }
    public GetAssignmentByIdQuery(Guid id)
    {
        Id = id;
    }
}
