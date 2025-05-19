using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Entities;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;

namespace HrApp.Infrastructure.Authorizations;

public class AssignmentAuthorizationService(IUserContext userContext, 
    ITeamRepository teamRepository) : IAssignmentAuthorizationService
{
    private readonly IUserContext _userContext = userContext;
    private readonly ITeamRepository _teamRepository = teamRepository;

    public async Task<bool> Authorize(ResourceOperation operation, Assignment assignment)
    {
        var user = _userContext.GetCurrentUser();
        if (user == null || !user.IsInRole("TeamLeader"))
            return false;

        var team = await _teamRepository.GetTeamForUser(Guid.Parse(user.id));

        if(team == null)
            return false;

        if (team.Id.ToString() != assignment.AssignedToTeamId.ToString())
            return false;

        string twoFA = user.twoFA;
        
        if (operation == ResourceOperation.Update)
        {
            if (twoFA == "false")
                return false;

            return true;
        }
        return false;
    }
}
