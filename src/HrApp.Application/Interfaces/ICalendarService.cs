using Google.Apis.Calendar.v3;

namespace HrApp.Application.Interfaces;

public interface ICalendarService
{
    public string CalendarId { get; set; }
    public CalendarService GetCalendarService();
}
