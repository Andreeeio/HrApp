using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IExellImportRepository
{
    Task AddImportAsync(ExcelImport exellImport);
}
