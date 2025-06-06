using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class OfferRepository : IOfferRepository
{
    private readonly HrAppContext _dbContext;

    public OfferRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Offer>> GetAllOffersAsync()
    {
        return await _dbContext.Offer.ToListAsync();
    }
    public async Task CreateOfferAsync(Offer offer)
    {
        _dbContext.Offer.Add(offer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateCandidateAsync(Candidate candidate)
    {
        _dbContext.Candidate.Add(candidate);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateJobApplicationAsync(JobApplication jobApplication)
    {
        _dbContext.JobApplication.Add(jobApplication);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Offer?> GetOfferWithApplicationsAsync(Guid offerId)
    {
        return await _dbContext.Offer
            .Include(o => o.JobApplications.Where(s => s.Status == "Received"))
                .ThenInclude(a => a.Candidate)
            .FirstOrDefaultAsync(o => o.Id == offerId);
    }

    public async Task<JobApplication?> GetJobApplicationAsync(Guid id)
    {
        return await _dbContext.JobApplication
            .Include(a => a.Candidate)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

}
