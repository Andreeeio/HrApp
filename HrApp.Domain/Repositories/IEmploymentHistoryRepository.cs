using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IEmploymentHistoryRepository
{
    public Task<List<EmploymentHistory>> GetExpiringContractsAsync(DateOnly dateFrom, DateOnly dateTo);
    public Task<List<EmploymentHistory>> GetEmploymentHistoriesForUserAsync(Guid userId);
    public Task<EmploymentHistory?> GetLatestEmploymentHistoryForUserAsync(Guid id);
    public Task AddEmploymentHistoryAsync(EmploymentHistory employmentHistory);
    public Task SaveChangesAsync();
}
