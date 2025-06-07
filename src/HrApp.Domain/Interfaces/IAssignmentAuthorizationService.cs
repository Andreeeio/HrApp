using HrApp.Domain.Constants;
using HrApp.Domain.Entities;

namespace HrApp.Domain.Interfaces;

public interface IAssignmentAuthorizationService
{
    public Task<bool> Authorize(ResourceOperation operation, Assignment assignment);
}
