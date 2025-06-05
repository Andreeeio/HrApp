using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;

namespace HrApp.Infrastructure.Repositories;

public class ExellImportRepository : IExellImportRepository
{
    private readonly HrAppContext _dbContext;

    public ExellImportRepository(HrAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddImportAsync(ExellImport exellImport)
    {
        _dbContext.ExellImports.Add(exellImport);
        return _dbContext.SaveChangesAsync();
    }
}
