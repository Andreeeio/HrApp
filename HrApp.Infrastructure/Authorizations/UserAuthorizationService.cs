using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Interfaces;

namespace HrApp.Infrastructure.Authorizations;

public class UserAuthorizationService(IUserContext userContext) : IUserAuthorizationService
{
    private readonly IUserContext _userContext = userContext;
    public bool Authorize(ResourceOperation operation)
    {
        var user = _userContext.GetCurrentUser();
        if(user == null)
            return false;

        return false;
    }
}
