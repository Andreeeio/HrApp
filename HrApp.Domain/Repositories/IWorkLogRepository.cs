using HrApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Domain.Repositories
{
    public interface IWorkLogRepository
    {
        public Task<List<WorkLog>> GetWorkLogsByUserId(Guid userId);
        public Task<WorkLog> GetWorkLogById(Guid workLogId);
        public Task AddWorkLog(WorkLog workLog);
        public Task UpdateWorkLog(WorkLog workLog);
    }
}
