using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;

namespace HrApp.Infrastructure.Repositories;

class ApiLogRepository : IApiLogRepository
{
    private readonly HrAppContext _dbContext;
    
    public ApiLogRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddLogAsync(ApiLog apiLog)
    {
        _dbContext.ApiLogs.Add(apiLog);
        await _dbContext.SaveChangesAsync();
    }
}
