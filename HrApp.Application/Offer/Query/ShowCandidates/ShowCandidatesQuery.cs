using HrApp.Application.Offer.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Offer.Query.ShowCandidates
{
    public class ShowCandidatesQuery : IRequest<List<JobApplicationWithCandidateDto>>
    {
        public Guid OfferId { get; }

        public ShowCandidatesQuery(Guid offerId)
        {
            OfferId = offerId;
        }
    }
}
