using HrApp.Domain.Entities;

namespace HrApp.Application.Teams.DTO;

public class TeamDTO
{
    public Guid Id { get; set; }
    public Guid TeamLeaderId { get; set; }
    public string Name { get; set; } = default!;
    public List<User>? Employers { get; set; } = default!;
    public List<Assignment>? Assignments { get; set; } = default!;
    public List<AnonymousFeedback>? AnonymousFeedbacks { get; set; } = default!;
    public Guid DepartmentId { get; set; }
}
