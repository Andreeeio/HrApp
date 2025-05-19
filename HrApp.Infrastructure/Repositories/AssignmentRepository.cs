using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly HrAppContext _dbContext;

    public AssignmentRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAssignment(Assignment assignment)
    {
        await _dbContext.Assignment.AddAsync(assignment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Assignment>> GetAllAssignmentsForTeam(Guid TeamId)
    {
        return await _dbContext.Assignment
            .Where(t => t.AssignedToTeamId == TeamId && t.IsEnded == false)
            .ToListAsync();
    }

    public async Task<List<Assignment>> GetActiveAssignments()
    {
        return await _dbContext.Assignment
            .Where(a => a.EndDate > DateTime.UtcNow && a.EndDate <= DateTime.UtcNow.AddDays(1) && a.IsEnded == false)
            .ToListAsync();
    }
    public async Task<List<Assignment>> GetFreeAssignments()
    {
        return await _dbContext.Assignment
            .Where(a => a.EndDate > DateTime.UtcNow && a.IsEnded == false && a.AssignedToTeamId == null)
            .ToListAsync();
    }

    public async Task<List<Assignment>> GetNotFreeAssignments()
    {
        return await _dbContext.Assignment
            .Where(a => a.EndDate > DateTime.UtcNow && a.IsEnded == false)
            .ToListAsync();
    }

    public async Task<Assignment?> GetAssignmentByIdAsync(Guid id)
    {
        return await _dbContext.Assignment.FindAsync(id);
    }
    public void Update(Assignment assignment)
    {
        _dbContext.Assignment.Update(assignment);
    }
    public async Task SaveChangesAsync()
    {

        await _dbContext.SaveChangesAsync();

    }
}
