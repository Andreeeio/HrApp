using HrApp.Domain.Constants;

namespace HrApp.Domain.Interfaces;

public interface ITeamAuthorizationService
{
    public bool Authorize(ResourceOperation operation);
}
