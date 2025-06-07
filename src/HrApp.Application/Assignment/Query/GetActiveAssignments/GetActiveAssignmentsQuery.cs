using HrApp.Application.Assignment.DTO;
using MediatR;

namespace HrApp.Application.Assignment.Query.GetActiveAssignments;

public class GetActiveAssignmentsQuery : IRequest<List<AssignmentDTO>>
{
}
