using HrApp.Application.Users.DTO;
using MediatR;

namespace HrApp.Application.Users.Query.GetRoleForUser;

public class GetRoleForUserCommand(string email) : IRequest<List<string>>
{
    public string Email { get; set; } = email;
}
