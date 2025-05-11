using AutoMapper;
using HrApp.Application.Offer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Mappings
{
    public class CreateOfferCommand : Profile
    {
        public CreateOfferCommand()
        {
            CreateMap<HrApp.Domain.Entities.Offer, OfferDTO>();
            CreateMap<OfferDTO, HrApp.Domain.Entities.Offer>();
        }
    }
}
