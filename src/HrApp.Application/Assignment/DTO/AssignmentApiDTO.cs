namespace HrApp.Application.Assignment.DTO;

public class AssignmentApiDTO
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsEnded { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? AssignedToTeamId { get; set; }
    public int DifficultyLevel { get; set; }
}
