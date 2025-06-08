using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class WorkLogExportHistoryRepository : IWorkLogExportHistoryRepository
{
    private readonly HrAppContext _dbContext;

    public WorkLogExportHistoryRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(WorkLogExportHistory history)
    {
        _dbContext.WorkLogExportHistory.Add(history);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<WorkLogExportHistory>> GetByExportedForUserId(Guid userId)
    {
        return await _dbContext.WorkLogExportHistory
            .Where(h => h.ExportedForUserId == userId)
            .ToListAsync();
    }
}
