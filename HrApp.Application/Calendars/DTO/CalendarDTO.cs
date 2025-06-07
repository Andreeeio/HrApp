namespace HrApp.Application.Calendars.DTO;

public class CalendarDTO
{
    public string Title { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = default!;
}
