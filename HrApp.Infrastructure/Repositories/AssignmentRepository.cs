using DocumentFormat.OpenXml.InkML;
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

    public async Task AddAssignmentAsync(Assignment assignment)
    {
        _dbContext.Assignment.Add(assignment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Assignment>> GetAllAssignmentsForTeamAsync(Guid TeamId)
    {
        return await _dbContext.Assignment
            .Where(t => t.AssignedToTeamId == TeamId && t.IsEnded == false)
            .ToListAsync();
    }

    public async Task<List<Assignment>> GetActiveAssignmentsAsync()
    {
        return await _dbContext.Assignment
            .Where(a => a.EndDate > DateTime.UtcNow && a.EndDate <= DateTime.UtcNow.AddDays(1) && a.IsEnded == false)
            .ToListAsync();
    }

    public async Task<List<Assignment>> GetAssignmentsAsync(bool onlyFree)
    {
        var query = _dbContext.Assignment
            .Where(a => a.EndDate > DateTime.UtcNow && a.IsEnded == false);

        if (onlyFree)
        {
            query = query.Where(a => a.AssignedToTeamId == null);
        }

        return await query.ToListAsync();
    }

    public async Task<Assignment?> GetAssignmentByIdAsync(Guid id)
    {
        return await _dbContext.Assignment.FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {

        await _dbContext.SaveChangesAsync();

    }

    public async Task<List<Assignment>> GetApiAssignmentsAsync(string? name, bool? isEnded, Guid? assignedToTeamId, int? difficultyLevel, CancellationToken cancellationToken)
    {
        var query = _dbContext.Assignment
               .Include(a => a.AssignedToTeam)
               .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(a => a.Name.Contains(name));

        if (isEnded.HasValue)
            query = query.Where(a => a.IsEnded == isEnded.Value);

        if (assignedToTeamId.HasValue)
            query = query.Where(a => a.AssignedToTeamId == assignedToTeamId.Value);

        if (difficultyLevel.HasValue)
            query = query.Where(a => a.DifficultyLevel == difficultyLevel.Value);

        return await query.ToListAsync(cancellationToken);
    }
}
