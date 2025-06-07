using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Infrastructure.Repositories
{
    public class WorkLogRepository : IWorkLogRepository
    {
        private readonly HrAppContext dbContext;
        public WorkLogRepository(HrAppContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task<List<WorkLog>> GetWorkLogsByUserIdAsync(Guid userId)
        {
            return dbContext.WorkLog
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }
        public async Task<WorkLog?> GetWorkLogByIdAsync(Guid workLogId)
        {
            return await dbContext.WorkLog
                .FirstOrDefaultAsync(w => w.Id == workLogId);
        }
        public async Task AddWorkLogAsync(WorkLog workLog)
        {
            dbContext.WorkLog.Add(workLog);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateWorkLogAsync(WorkLog workLog)
        {
            dbContext.WorkLog.Update(workLog);
            await dbContext.SaveChangesAsync();
        }

    }
}
