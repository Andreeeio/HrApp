using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface ISalaryHistoryRepository
{
    Task<List<SalaryHistory>> GetSalaryHistoryForUserAsync(Guid userId, int? howMany = null);
    Task AddAsync(SalaryHistory history);
}
