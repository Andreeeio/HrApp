using HrApp.Application.Feedback.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Feedback.Query
{
    public class GetAnonymousFeedbacksForTeamQuery : IRequest<List<AnonymousFeedbackDTO>>
    {
        public Guid TeamId { get; set; }

        public GetAnonymousFeedbacksForTeamQuery(Guid teamId)
        {
            TeamId = teamId;
        }
    }
}
