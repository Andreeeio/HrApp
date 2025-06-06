using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IWorkLogExportHistoryRepository
{
    Task AddAsync(WorkLogExportHistory history);
    Task<List<WorkLogExportHistory>> GetByExportedForUserId(Guid userId);
}
