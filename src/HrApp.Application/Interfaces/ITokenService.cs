using HrApp.Application.Users.DTO;
using HrApp.Domain.Entities;

namespace HrApp.Application.Interfaces;

public interface ITokenService
{
    string GetToken(User user, bool ipVer);
    string GetToken(CurrentUser user, bool ipVer);
}
