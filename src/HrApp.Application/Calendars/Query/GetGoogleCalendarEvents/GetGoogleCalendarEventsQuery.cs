using HrApp.Application.Calendars.DTO;
using MediatR;

namespace HrApp.Application.Calendars.Query.GetGoogleCalendarEvents;

public class GetGoogleCalendarEventsQuery : IRequest<List<CalendarDTO>>
{
}
