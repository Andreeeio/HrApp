using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IFeedbackRepository
{
    Task AddAnonymousFeedbackAsync(AnonymousFeedback feedback);
    Task DeleteFeedbackAsync(Guid id);
    Task<List<AnonymousFeedback>> GetAnonymousFeedbacksForTeamAsync(Guid teamId);
}
