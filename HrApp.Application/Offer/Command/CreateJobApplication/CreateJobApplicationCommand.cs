using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Offer.Command.CreateJobApplication
{
    public class CreateJobApplicationCommand : IRequest
    {
        public Guid OfferId { get; set; }
        public Guid CandidateId { get; set; }
        public DateOnly ApplicationDate { get; set; }
        public string Status { get; set; } = default!;
        public IFormFile CvFile { get; set; } = default!;
        public string CvLink { get; set; } = "Pending";
    }
}
