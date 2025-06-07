using HrApp.Application.Assignment.DTO;
using MediatR;

namespace HrApp.Application.Assignment.Query.GetAssignmentsForApi;

public class GetAssignmentsForApiQuery : IRequest<List<AssignmentApiDTO>>
{
    public string? Name { get; set; } = default!;
    public bool? IsEnded { get; set; }
    public Guid? AssignedToTeamId { get; set; }
    public int? DifficultyLevel { get; set; }
}
