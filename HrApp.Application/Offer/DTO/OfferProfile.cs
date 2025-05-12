using AutoMapper;
using HrApp.Application.Offer.DTO;

namespace HrApp.Application.Mappings;

public class OfferMappingProfile : Profile
{
    public OfferMappingProfile()
    {
        CreateMap<Domain.Entities.Offer, OfferDTO>();
    }
}
