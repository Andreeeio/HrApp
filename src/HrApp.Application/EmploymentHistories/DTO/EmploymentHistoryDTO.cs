namespace HrApp.Application.EmploymentHistories.DTO;

public class EmploymentHistoryDTO
{
    public string Position { get; set; } = default!;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
