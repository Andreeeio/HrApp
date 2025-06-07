using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface ISalaryRepository
{
    Task AddPaidAsync(Paid paid);
    Task<Paid?> GetPaidByUserIdAsync(Guid userid);
    Task<Paid?> GetPaidByIdAsync(Guid paidid);
    Task UpdatePaidAsync(Paid paid);
    Task<List<Paid>> GetAllPaidAsync();
}
