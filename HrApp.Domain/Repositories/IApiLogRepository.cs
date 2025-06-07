using HrApp.Domain.Entities;

namespace HrApp.Domain.Repositories;

public interface IApiLogRepository 
{
    Task AddLogAsync(ApiLog apiLog);
}
