namespace HrApp.Domain.Entities;

public class Offer
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; } 
    public virtual Team Team { get; set; } = default!; 
    public string PositionName { get; set; } = default!;
    public float Salary { get; set; }
    public string Description { get; set; } = default!;
    public DateOnly AddDate { get; set; }
    public List<JobApplication> JobApplications { get; set; } = default!;
}
