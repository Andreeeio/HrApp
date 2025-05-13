using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class OfferRepository : IOfferRepository
{
    private readonly HrAppContext dbContext;

    public OfferRepository(HrAppContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Offer>> GetAllOffers()
    {
        return await dbContext.Offer.ToListAsync();
    }
    public async Task CreateOffer(Offer offer)
    {
        await dbContext.Offer.AddAsync(offer);
        await dbContext.SaveChangesAsync();
    }

    public async Task CreateCandidate(Candidate candidate)
    {
        await dbContext.Candidate.AddAsync(candidate);
        await dbContext.SaveChangesAsync();
    }

    public async Task CreateJobApplication(JobApplication jobApplication)
    {
        await dbContext.JobApplication.AddAsync(jobApplication);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Offer?> GetOfferWithApplications(Guid offerId)
    {
        return await dbContext.Offer
            .Include(o => o.JobApplications)
                .ThenInclude(a => a.Candidate)
            .FirstOrDefaultAsync(o => o.Id == offerId);
    }

}
