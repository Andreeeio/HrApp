using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface ICalendarRepository
{
    public Task AddCalendarEvent(Calendar calendar);
}
