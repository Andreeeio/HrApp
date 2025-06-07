using HrApp.Application.Assignment.DTO;
using MediatR;

namespace HrApp.Application.Assignment.Command.AddAssignment;

public class AddAssignmentCommand : AssignmentDTO, IRequest
{
}