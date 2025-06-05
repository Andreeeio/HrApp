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
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly HrAppContext _context;
        public FeedbackRepository(HrAppContext context)
        {
            _context = context;
        }
        public async Task AddAnonymousFeedback(AnonymousFeedback feedback)
        {
            await _context.AnonymousFeedback.AddAsync(feedback);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteFeedback(Guid id)
        {
            var feedback = await _context.AnonymousFeedback.FindAsync(id);
            if (feedback != null)
            {
                _context.AnonymousFeedback.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<AnonymousFeedback>> GetAnonymousFeedbacksForTeam(Guid teamId)
        {
            return await _context.AnonymousFeedback
                .Where(f => f.TeamId == teamId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

    }
}
