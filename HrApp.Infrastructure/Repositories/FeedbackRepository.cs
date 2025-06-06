using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories; 

public class FeedbackRepository : IFeedbackRepository
{
    private readonly HrAppContext _dbContext;

    public FeedbackRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAnonymousFeedbackAsync(AnonymousFeedback feedback)
    {
        _dbContext.AnonymousFeedbacks.Add(feedback);
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteFeedbackAsync(Guid id)
    {
        var feedback = await _dbContext.AnonymousFeedbacks.FindAsync(id);
        if (feedback != null)
        {
            _dbContext.AnonymousFeedbacks.Remove(feedback);
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task<List<AnonymousFeedback>> GetAnonymousFeedbacksForTeamAsync(Guid teamId)
    {
        return await _dbContext.AnonymousFeedbacks
            .Where(f => f.TeamId == teamId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }
}
