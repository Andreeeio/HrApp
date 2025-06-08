using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HrApp.Infrastructure.Repositories;

public class SalaryHistoryRepository : ISalaryHistoryRepository
{
    private readonly HrAppContext _dbContext;

    public SalaryHistoryRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<SalaryHistory>> GetSalaryHistoryForUserAsync(Guid userId, int? howMany = null)
    {
        var salaryHistory = _dbContext.SalaryHistory
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.MonthNYear);

        if (howMany == null || howMany <= 0)
            return await salaryHistory.ToListAsync();

        return await salaryHistory.Take(howMany.Value).ToListAsync();
    }
    public Task AddAsync(SalaryHistory history)
    {
        _dbContext.SalaryHistory.Add(history);
        return _dbContext.SaveChangesAsync();
    }
}
