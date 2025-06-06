using AutoMapper;
using HrApp.Application.Offer.DTO;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Offer.Query.GetAllOffers;

public class GetAllOffersQueryHandler : IRequestHandler<GetAllOffersQuery, List<OfferDTO>>
{
    private readonly ILogger<GetAllOffersQueryHandler> _logger;
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;

    public GetAllOffersQueryHandler(
        ILogger<GetAllOffersQueryHandler> logger,
        IOfferRepository offerRepository,
        IMapper mapper)
    {
        _logger = logger;
        _offerRepository = offerRepository;
        _mapper = mapper;
    }

    public async Task<List<OfferDTO>> Handle(GetAllOffersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all offers");

        var offers = await _offerRepository.GetAllOffersAsync();
        var dto = _mapper.Map<List<OfferDTO>>(offers);

        return dto;
    }
}
