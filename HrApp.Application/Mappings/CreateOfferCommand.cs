using AutoMapper;
using HrApp.Application.Offer.DTO;

namespace HrApp.Application.Mappings;

public class CreateOfferCommand : Profile
{
    public CreateOfferCommand()
    {
        CreateMap<HrApp.Domain.Entities.Offer, OfferDTO>();
        CreateMap<OfferDTO, HrApp.Domain.Entities.Offer>();
    }
}
