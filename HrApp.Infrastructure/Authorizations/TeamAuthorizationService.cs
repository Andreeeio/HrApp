using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Interfaces;

namespace HrApp.Infrastructure.Authorizations;

public class TeamAuthorizationService(IUserContext userContext) : ITeamAuthorizationService
{
    private readonly IUserContext _userContext = userContext;
    public bool Authorize(ResourceOperation operation)
    {
        var user = _userContext.GetCurrentUser();
        if (user == null)
            return false;

        if(operation == ResourceOperation.Read
            && (user.IsInRole(Roles.TeamLeader.ToString()) || user.IsInRole(Roles.Hr.ToString()) || user.IsInRole(Roles.Ceo.ToString())))
        {
            return true;
        }
        else if (operation == ResourceOperation.Update
            && (user.IsInRole(Roles.TeamLeader.ToString()) || user.IsInRole(Roles.Hr.ToString()) || user.IsInRole(Roles.Ceo.ToString())))
        {
            return true;
        }

        return false; // Placeholder for actual authorization logic
    }
}
