using HrApp.Domain.Entities;

namespace HrApp.Application.Interfaces;

public interface ITokenService
{
    string GetToken(User user);
}
