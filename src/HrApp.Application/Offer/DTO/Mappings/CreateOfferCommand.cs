using AutoMapper;
using HrApp.Application.Offer.DTO;

namespace HrApp.Application.Offer.DTO.Mappings;

public class CreateOfferCommand : Profile
{
    public CreateOfferCommand()
    {
        CreateMap<Domain.Entities.Offer, OfferDTO>();
        
        CreateMap<OfferDTO, Domain.Entities.Offer>();
    }
}
