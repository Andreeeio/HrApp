using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class WorkLogRepository : IWorkLogRepository
{
    private readonly HrAppContext _dbContext;
    public WorkLogRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task<List<WorkLog>> GetWorkLogsByUserIdAsync(Guid userId)
    {
        return _dbContext.WorkLog
            .Where(w => w.UserId == userId)
            .ToListAsync();
    }
    public async Task<WorkLog?> GetWorkLogByIdAsync(Guid workLogId)
    {
        return await _dbContext.WorkLog
            .FirstOrDefaultAsync(w => w.Id == workLogId);
    }
    public async Task AddWorkLogAsync(WorkLog workLog)
    {
        _dbContext.WorkLog.Add(workLog);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateWorkLogAsync(WorkLog workLog)
    {
        _dbContext.WorkLog.Update(workLog);
        await _dbContext.SaveChangesAsync();
    }
}
