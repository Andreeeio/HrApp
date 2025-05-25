using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IEmploymentHistoryRepository
{
    public Task<List<EmploymentHistory>> GetExpiringContractsAsync(DateOnly dateFrom, DateOnly dateTo);
}
