using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class SalaryRepository : ISalaryRepository
{
    private readonly HrAppContext _dbContext;
    public SalaryRepository(HrAppContext context)
    {
        _dbContext = context;
    }

    public Task AddPaidAsync(Paid paid)
    {
        _dbContext.Paid.AddAsync(paid);
        return _dbContext.SaveChangesAsync();
    }

    public async Task<Paid?> GetPaidByUserIdAsync(Guid userid)
    {
        return await _dbContext.Paid.FirstOrDefaultAsync(x => x.UserId == userid);
    }

    public async Task<Paid?> GetPaidByIdAsync(Guid paidid)
    {
        return await _dbContext.Paid.FirstOrDefaultAsync(x => x.Id == paidid);
    }

    public async Task UpdatePaidAsync(Paid paid)
    {
        _dbContext.Paid.Update(paid);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Paid>> GetAllPaidAsync()
    {
        return await _dbContext.Paid.ToListAsync();
    }
}
