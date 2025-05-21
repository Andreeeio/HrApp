using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class EmployeeRateRepository : IEmployeeRateRepository
{
    private readonly HrAppContext _dbContext;
    public EmployeeRateRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddRatesAsync(List<EmployeeRate> rates)
    {
        _dbContext.EmployeeRate.AddRange(rates);
        await _dbContext.SaveChangesAsync();
    }
    public Task<IEnumerable<EmployeeRate>> GetEmployeeRatesByUserId(Guid userid)
    {
        return Task.FromResult(_dbContext.EmployeeRate.Where(x => x.EmployeeId == userid).AsEnumerable());
    }

}
