using AutoMapper;
using HrApp.Application.Offer.DTO;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Offer.Query.ShowCandidates;

public class ShowCandidatesQueryHandler : IRequestHandler<ShowCandidatesQuery, List<JobApplicationWithCandidateDto>>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ShowCandidatesQueryHandler> _logger;

    public ShowCandidatesQueryHandler(IOfferRepository offerRepository, IMapper mapper, ILogger<ShowCandidatesQueryHandler> logger)
    {
        _offerRepository = offerRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<JobApplicationWithCandidateDto>> Handle(ShowCandidatesQuery request, CancellationToken cancellationToken)
    {
        var offer = await _offerRepository.GetOfferWithApplicationsAsync(request.OfferId);
        if (offer == null)
        {
            _logger.LogWarning("Offer with ID {OfferId} not found.", request.OfferId);
            return new List<JobApplicationWithCandidateDto>();
        }

        var jobApplications = offer.JobApplications;

        return _mapper.Map<List<JobApplicationWithCandidateDto>>(jobApplications);
    }
}
