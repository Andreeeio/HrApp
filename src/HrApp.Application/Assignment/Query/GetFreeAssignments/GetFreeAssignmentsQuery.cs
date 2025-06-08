using HrApp.Application.Assignment.DTO;
using MediatR;

namespace HrApp.Application.Assignment.Query.GetFreeAssignments;

public class GetFreeAssignmentsQuery : IRequest<List<AssignmentDTO>>
{
}
