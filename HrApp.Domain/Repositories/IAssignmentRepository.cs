using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IAssignmentRepository
{
    Task<List<Assignment>> GetApiAssignmentsAsync(string? name,
        bool? isEnded,
        Guid? assignedToTeamId,
        int? difficultyLevel,
        CancellationToken cancellationToken);

    Task<List<Assignment>> GetAllAssignmentsForTeamAsync(Guid TeamId);
    Task<List<Assignment>> GetActiveAssignmentsAsync();
    Task<List<Assignment>> GetAssignmentsAsync(bool onlyFree);
    Task AddAssignmentAsync(Assignment assignment);
    Task<Assignment?> GetAssignmentByIdAsync(Guid id);
    Task SaveChangesAsync();
}
