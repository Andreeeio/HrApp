using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface ISalaryHistoryRepository
{
    public Task<List<SalaryHistory>> GetSalaryHistoryForUser(Guid userId, int? howMany = null);
    Task AddAsync(SalaryHistory history);
}
