using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IEmployeeRateRepository
{
    Task AddRatesAsync(List<EmployeeRate> rates);
    Task<List<EmployeeRate>> GetRatesForUserAsync(Guid userId);
}
