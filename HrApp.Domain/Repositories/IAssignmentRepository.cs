using HrApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Domain.Repositories
{
    public interface IAssignmentRepository
    {
        public Task<List<Assignment>> GetAllAssignmentsForTeam(Guid TeamId);
        public Task AddAssignment(Assignment assignment);
    }
}
