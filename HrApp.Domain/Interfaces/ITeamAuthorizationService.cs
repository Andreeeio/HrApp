using HrApp.Domain.Constants;

namespace HrApp.Domain.Interfaces;

public interface ITeamAuthorizationService
{
    bool Authorize(ResourceOperation operation);
}
