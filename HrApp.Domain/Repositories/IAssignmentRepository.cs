using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IAssignmentRepository
{
    public Task<List<Assignment>> GetAllAssignmentsForTeam(Guid TeamId);
    public Task<List<Assignment>> GetActiveAssignments();
    public Task<List<Assignment>> GetFreeAssignments();
    public Task<List<Assignment>> GetNotFreeAssignments();
    public Task AddAssignment(Assignment assignment);
    Task<Assignment> GetAssignmentByIdAsync(Guid id);
    Task SaveChangesAsync();
}
