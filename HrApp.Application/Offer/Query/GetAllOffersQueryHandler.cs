using AutoMapper;
using HrApp.Application.Offer.DTO;
using HrApp.Application.Offer.Query;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Offers.Query.GetAllOffers;

public class GetAllOffersQueryHandler(
    ILogger<GetAllOffersQueryHandler> logger,
    IOfferRepository repository,
    IMapper mapper)
    : IRequestHandler<GetAllOffersQuery, List<OfferDTO>>
{
    private readonly ILogger<GetAllOffersQueryHandler> _logger = logger;
    private readonly IOfferRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<OfferDTO>> Handle(GetAllOffersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all offers");

        var offers = await _repository.GetAllOffers();
        var dto = _mapper.Map<List<OfferDTO>>(offers);

        return dto;
    }
}
