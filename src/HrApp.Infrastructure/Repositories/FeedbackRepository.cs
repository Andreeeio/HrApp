using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories; 

public class FeedbackRepository : IFeedbackRepository
{
    private readonly HrAppContext _context;
    public FeedbackRepository(HrAppContext context)
    {
        _context = context;
    }
    public async Task AddAnonymousFeedbackAsync(AnonymousFeedback feedback)
    {
        await _context.AnonymousFeedback.AddAsync(feedback);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteFeedbackAsync(Guid id)
    {
        var feedback = await _context.AnonymousFeedback.FindAsync(id);
        if (feedback != null)
        {
            _context.AnonymousFeedback.Remove(feedback);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<List<AnonymousFeedback>> GetAnonymousFeedbacksForTeamAsync(Guid teamId)
    {
        return await _context.AnonymousFeedback
            .Where(f => f.TeamId == teamId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

}
