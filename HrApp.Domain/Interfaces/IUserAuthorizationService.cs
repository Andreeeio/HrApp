using HrApp.Domain.Constants;

namespace HrApp.Domain.Interfaces;

public interface IUserAuthorizationService
{
    bool Authorize(ResourceOperation operation);
}
