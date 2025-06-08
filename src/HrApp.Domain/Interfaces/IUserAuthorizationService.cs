using HrApp.Domain.Constants;

namespace HrApp.Domain.Interfaces;

public interface IUserAuthorizationService
{
    public bool Authorize(ResourceOperation operation);
}
