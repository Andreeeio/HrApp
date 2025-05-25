using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class EmploymentHistoryRepository(HrAppContext dbContext) : IEmploymentHistoryRepository
{
    private readonly HrAppContext _dbContext = dbContext;
    public async Task<List<EmploymentHistory>> GetExpiringContractsAsync(DateOnly dateFrom, DateOnly dateTo)
    {
        var empH = await _dbContext.EmploymentHistory.
            Where(eh => eh.EndDate.HasValue && eh.EndDate.Value >= dateFrom && eh.EndDate.Value <= dateTo)
            .ToListAsync();

        return empH;
    }
}
