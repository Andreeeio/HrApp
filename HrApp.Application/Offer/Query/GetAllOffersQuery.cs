using HrApp.Application.Offer.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Offer.Query
{
    public class GetAllOffersQuery : IRequest<List<OfferDTO>>
    {
    }
}
