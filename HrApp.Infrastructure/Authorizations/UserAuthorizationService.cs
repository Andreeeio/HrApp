using Bogus.Extensions.Extras;
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

        string twoFA = user.twoFA;

        if (operation == ResourceOperation.Update)
        {
            if (twoFA == "false")
                return false;

            return true;
        }
        if (operation == ResourceOperation.Read)
        {
            if (twoFA == "false")
                return false;
         
            return true;
        }

        return false;
    }
}
