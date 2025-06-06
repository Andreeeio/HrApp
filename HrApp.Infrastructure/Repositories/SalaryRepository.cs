using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Repositories;

public class SalaryRepository : ISalaryRepository
{
    private readonly HrAppContext _dbcontext;
    public SalaryRepository(HrAppContext context)
    {
        _dbcontext = context;
    }

    public Task AddPaidAsync(Paid paid)
    {
        _dbcontext.Paid.AddAsync(paid);
        return _dbcontext.SaveChangesAsync();
    }

    public async Task<Paid?> GetPaidByUserIdAsync(Guid userid)
    {
        return await _dbcontext.Paid.FirstOrDefaultAsync(x => x.UserId == userid);
    }

    public async Task<Paid?> GetPaidByIdAsync(Guid paidid)
    {
        return await _dbcontext.Paid.FirstOrDefaultAsync(x => x.Id == paidid);
    }

    public async Task UpdatePaidAsync(Paid paid)
    {
        _dbcontext.Paid.Update(paid);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task<List<Paid>> GetAllPaidAsync()
    {
        return await _dbcontext.Paid.ToListAsync();
    }
}
