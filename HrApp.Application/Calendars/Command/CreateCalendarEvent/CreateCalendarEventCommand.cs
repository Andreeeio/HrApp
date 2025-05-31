using MediatR;

namespace HrApp.Application.Calendars.Command.CreateCalendarEvent;

public class CreateCalendarEventCommand : IRequest
{
    public string Title { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = default!;
}
