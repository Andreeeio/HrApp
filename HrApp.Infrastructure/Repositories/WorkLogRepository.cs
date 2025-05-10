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
        public Task<List<WorkLog>> GetWorkLogsByUserId(Guid userId)
        {
            return dbContext.WorkLog
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }
        public Task<WorkLog> GetWorkLogById(Guid workLogId)
        {
            return dbContext.WorkLog
                .FirstOrDefaultAsync(w => w.Id == workLogId);
        }
        public async Task AddWorkLog(WorkLog workLog)
        {
            dbContext.WorkLog.Add(workLog); // Użyj DbSet<WorkLog>
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateWorkLog(WorkLog workLog)
        {
            dbContext.WorkLog.Update(workLog); // Użyj DbSet<WorkLog>
            await dbContext.SaveChangesAsync();
        }

    }
}
