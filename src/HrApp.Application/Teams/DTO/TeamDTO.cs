using HrApp.Domain.Entities;

namespace HrApp.Application.Teams.DTO;

public class TeamDTO
{
    public Guid Id { get; set; }
    public Guid TeamLeaderId { get; set; }
    public string Name { get; set; } = default!;
    public List<User> Employers { get; set; } = new List<User>();
    public List<HrApp.Domain.Entities.Assignment> Assignments { get; set; } = new List<HrApp.Domain.Entities.Assignment>();
    public List<AnonymousFeedback> AnonymousFeedbacks { get; set; } = new List<AnonymousFeedback>();
    public Guid DepartmentId { get; set; }
}
