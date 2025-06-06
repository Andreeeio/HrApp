using HrApp.Application.Offer.DTO;
using MediatR;

namespace HrApp.Application.Offer.Query.ShowCandidates;

public class ShowCandidatesQuery : IRequest<List<JobApplicationWithCandidateDto>>
{
    public Guid OfferId { get; }

    public ShowCandidatesQuery(Guid offerId)
    {
        OfferId = offerId;
    }
}
