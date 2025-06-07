using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IOfferRepository
{
    Task<List<Offer>> GetAllOffersAsync();
    Task CreateOfferAsync(Offer offer);
    Task CreateCandidateAsync(Candidate candidate);
    Task CreateJobApplicationAsync(JobApplication jobApplication);
    Task<Offer?> GetOfferWithApplicationsAsync(Guid offerId);
    Task<JobApplication?> GetJobApplicationAsync(Guid id);
    Task SaveChangesAsync();
}
