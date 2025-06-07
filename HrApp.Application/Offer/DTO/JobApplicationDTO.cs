using HrApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Offer.DTO
{
    public class JobApplicationDTO
    {
        public Guid Id { get; set; }
        public Guid OfferID { get; set; }
        public Guid CandidateId { get; set; }
        public DateOnly ApplicationDate { get; set; }
        public string Status { get; set; } = default!;
        public string CvLink { get; set; } = default!;
    }
}
