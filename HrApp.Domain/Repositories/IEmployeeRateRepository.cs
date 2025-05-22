using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IEmployeeRateRepository
{
    public Task AddRatesAsync(List<EmployeeRate> rates);
    Task<IEnumerable<EmployeeRate>> GetEmployeeRatesByUserId(Guid userid);
    Task<List<EmployeeRate>> GetRatesForUserAsync(Guid userId);
}
