using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IEmploymentHistoryRepository
{
    Task<List<EmploymentHistory>> GetExpiringContractsAsync(DateOnly dateFrom, DateOnly dateTo);
    Task<List<EmploymentHistory>> GetEmploymentHistoriesForUserAsync(Guid userId);
    Task<EmploymentHistory?> GetLatestEmploymentHistoryForUserAsync(Guid id);
    Task AddEmploymentHistoryAsync(EmploymentHistory employmentHistory);
    Task SaveChangesAsync();
}
