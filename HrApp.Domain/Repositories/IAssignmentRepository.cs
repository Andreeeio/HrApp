using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IAssignmentRepository
{
    public Task<List<Assignment>> GetAllAssignmentsForTeam(Guid TeamId);
    public Task<List<Assignment>> GetActiveAssignments();
    public Task AddAssignment(Assignment assignment);

}
