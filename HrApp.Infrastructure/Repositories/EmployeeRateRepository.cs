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
    public async Task<List<EmployeeRate>> GetRatesForUserAsync(Guid userId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var startOfMonth = new DateOnly(today.Year, today.Month, 1);
        var startOfNextMonth = startOfMonth.AddMonths(1);

        var rates = await _dbContext.EmployeeRate
            .Where(x => x.EmployeeId == userId && x.RateDate >= startOfMonth && x.RateDate < startOfNextMonth)
            .Include(x => x.RatedBy)
            .ToListAsync();

        return rates;
    }

}
