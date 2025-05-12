using HrApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Offer.DTO
{
    public class OfferDTO
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; } // Relacja do Team
        public string PositionName { get; set; } = default!;
        public float Salary { get; set; }
        public string Description { get; set; } = default!;
        public DateOnly AddDate { get; set; }
    }
}
