using AutoMapper;
using HrApp.Application.Offer.DTO;

namespace HrApp.Application.Mappings;

public class OfferMappingProfile : Profile
{
    public OfferMappingProfile()
    {
        CreateMap<HrApp.Domain.Entities.Offer, OfferDTO>();
    }
}
