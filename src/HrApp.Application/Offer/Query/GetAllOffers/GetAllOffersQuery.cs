using HrApp.Application.Offer.DTO;
using MediatR;

namespace HrApp.Application.Offer.Query.GetAllOffers;

public class GetAllOffersQuery : IRequest<List<OfferDTO>>
{
}