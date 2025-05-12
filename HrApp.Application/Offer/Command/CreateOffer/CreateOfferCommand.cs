using HrApp.Application.Offer.DTO;
using MediatR;

namespace HrApp.Application.Offer.Command.CreateOffer
{
    public class CreateOfferCommand : OfferDTO,IRequest 
    {
    }
}
