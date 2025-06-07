using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IWorkLogRepository
{
    Task<List<WorkLog>> GetWorkLogsByUserIdAsync(Guid userId);
    Task<WorkLog?> GetWorkLogByIdAsync(Guid workLogId);
    Task AddWorkLogAsync(WorkLog workLog);
    Task UpdateWorkLogAsync(WorkLog workLog);
}
