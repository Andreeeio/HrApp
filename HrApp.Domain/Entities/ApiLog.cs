namespace HrApp.Domain.Entities;

public class ApiLog
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool? IsEnded { get; set; }
    public Guid? AssignedToTeamId { get; set; }
    public int? DifficultyLevel { get; set; }
    public DateTime CreatedAt { get; set; } 
}
