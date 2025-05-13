using HrApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Domain.Repositories
{
    public interface IOfferRepository
    {
        Task<List<Offer>> GetAllOffers();
        Task CreateOffer(Offer offer);
        Task CreateCandidate(Candidate candidate);
        Task CreateJobApplication(JobApplication jobApplication);
        Task<Offer?> GetOfferWithApplications(Guid offerId);
    }
}
