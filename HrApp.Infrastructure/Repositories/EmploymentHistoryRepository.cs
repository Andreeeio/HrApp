using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class EmploymentHistoryRepository : IEmploymentHistoryRepository
{
    private readonly HrAppContext _dbContext ;

    public EmploymentHistoryRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<EmploymentHistory>> GetExpiringContractsAsync(DateOnly dateFrom, DateOnly dateTo)
    {
        var empH = await _dbContext.EmploymentHistory
            .Where(eh => eh.EndDate.HasValue && eh.EndDate.Value >= dateFrom && eh.EndDate.Value <= dateTo)
            .ToListAsync();

        return empH;
    }

    public async Task<List<EmploymentHistory>> GetEmploymentHistoriesForUserAsync(Guid userId)
    {
        return await _dbContext.EmploymentHistory
            .Where(eh => eh.UserId == userId)
            .OrderBy(eh => eh.StartDate)
            .ToListAsync();
    }

    public async Task<EmploymentHistory?> GetLatestEmploymentHistoryForUserAsync(Guid id)
    {
        var empHist = await _dbContext.EmploymentHistory
            .Where(e => e.UserId == id)
            .OrderBy(e => e.EndDate != null) 
            .ThenByDescending(e => e.EndDate) 
            .FirstOrDefaultAsync();

        return empHist;
    }

    public async Task AddEmploymentHistoryAsync(EmploymentHistory employmentHistory)
    {
        _dbContext.EmploymentHistory.Add(employmentHistory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
