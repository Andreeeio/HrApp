using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IAssignmentRepository
{
    public Task<List<Assignment>> GetAllAssignmentsForTeam(Guid TeamId);
    public Task<List<Assignment>> GetActiveAssignments();
    public Task<List<Assignment>> GetFreeAssignments();
    public Task<List<Assignment>> GetNotFreeAssignments();
    public Task AddAssignment(Assignment assignment);
    public Task<Assignment?> GetAssignmentByIdAsync(Guid id);
    public void Update(Assignment assignment);
    public Task SaveChangesAsync();
}
