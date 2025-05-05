namespace HrApp.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public Guid TeamLeaderId { get; set; }
    public string Name { get; set; } = default!;
    public virtual User TeamLeader { get; set; } = default!;
    public List<User> Employers { get; set; } = new List<User>();
    public List<Assignment> Assignments { get; set; } = new List<Assignment>();
    public List<AnonymousFeedback> AnonymousFeedbacks { get; set; } = new List<AnonymousFeedback>();
    public Guid DepartmentId { get; set; }
    public virtual Department Department { get; set; } = default!;
}
