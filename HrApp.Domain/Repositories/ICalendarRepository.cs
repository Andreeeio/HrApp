using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface ICalendarRepository
{
    Task AddCalendarEventAsync(Calendar calendar);
}
