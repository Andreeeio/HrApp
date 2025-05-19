using HrApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Domain.Repositories
{
    public interface IFeedbackRepository
    {
        Task AddAnonymousFeedback(AnonymousFeedback feedback);
        Task DeleteFeedback(Guid id);
        Task<List<AnonymousFeedback>> GetAnonymousFeedbacksForTeam(Guid teamId);
    }
}
