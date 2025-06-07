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

    public Task AddImportAsync(ExcelImport exellImport)
    {
        _dbContext.ExcelImport.Add(exellImport);
        return _dbContext.SaveChangesAsync();
    }
}
