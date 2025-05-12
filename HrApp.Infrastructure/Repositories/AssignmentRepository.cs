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
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly HrAppContext dbContext;

        public AssignmentRepository(HrAppContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAssignment(Assignment assignment)
        {
            await dbContext.Assignment.AddAsync(assignment);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Assignment>> GetAllAssignmentsForTeam(Guid TeamId)
        {
            return await dbContext.Assignment
                .Where(t => t.AssignedToTeamId == TeamId)
                .ToListAsync();
        }
    }
}
