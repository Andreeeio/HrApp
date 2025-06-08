using HrApp.Application.Users.DTO;
using MediatR;

namespace HrApp.Application.Users.Query.GetAllUsers;

public class GetAllUsersQuery : IRequest<List<UserDTO>>
{
}
