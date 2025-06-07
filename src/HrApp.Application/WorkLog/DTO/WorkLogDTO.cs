namespace HrApp.Application.WorkLog.DTO;

public class WorkLogDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Hours { get; set; }
}