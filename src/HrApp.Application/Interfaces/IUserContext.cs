using HrApp.Application.Users.DTO;

namespace HrApp.Application.Interfaces;

public interface IUserContext
{
    public CurrentUser? GetCurrentUser();
}
