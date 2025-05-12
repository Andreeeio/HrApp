using HrApp.Application.Assignment.DTO;
using MediatR;

namespace HrApp.Application.Assignment.Query.GetAssignmentForTeam;

public class GetAssignmentForTeamQuery(Guid TeamId) : IRequest<List<AssignmentDTO>>
{
    public Guid AssignedToTeamId { get; set; } = TeamId;
}