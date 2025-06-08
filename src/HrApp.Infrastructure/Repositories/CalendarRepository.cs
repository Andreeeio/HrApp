using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;

namespace HrApp.Infrastructure.Repositories;

public class CalendarRepository : ICalendarRepository
{
    private readonly HrAppContext _dbContext;

    public CalendarRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddCalendarEventAsync(Calendar calendar)
    {
        if (calendar.Creator != null)
        {
            _dbContext.CalendarEventCreator.Add(calendar.Creator);
        }
        _dbContext.Calendar.Add(calendar);
        await _dbContext.SaveChangesAsync();
    }
}
