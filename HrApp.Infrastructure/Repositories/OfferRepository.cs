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

    public async Task<List<Offer>> GetAllOffersAsync()
    {
        return await dbContext.Offer.ToListAsync();
    }
    public async Task CreateOfferAsync(Offer offer)
    {
        dbContext.Offer.Add(offer);
        await dbContext.SaveChangesAsync();
    }

    public async Task CreateCandidateAsync(Candidate candidate)
    {
        dbContext.Candidate.Add(candidate);
        await dbContext.SaveChangesAsync();
    }

    public async Task CreateJobApplicationAsync(JobApplication jobApplication)
    {
        dbContext.JobApplication.Add(jobApplication);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Offer?> GetOfferWithApplicationsAsync(Guid offerId)
    {
        return await dbContext.Offer
            .Include(o => o.JobApplications.Where(s => s.Status == "Received"))
                .ThenInclude(a => a.Candidate)
            .FirstOrDefaultAsync(o => o.Id == offerId);
    }

    public async Task<JobApplication?> GetJobApplicationAsync(Guid id)
    {
        return await dbContext.JobApplication
            .Include(a => a.Candidate)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }

}
