namespace HrApp.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public Guid TeamLeaderId { get; set; }
    public virtual User TeamLeader { get; set; } = default!;
    public List<User> Employers { get; set; } = default!;
    public List<Assignment> Assignments { get; set; } = default!;
    public List<AnonymousFeedback> AnonymousFeedbacks { get; set; } = default!;
    public Guid DepartmentId { get; set; }
    public virtual Department Department { get; set; } = default!;
}
