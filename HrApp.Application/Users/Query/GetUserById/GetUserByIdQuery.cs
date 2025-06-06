using HrApp.Application.Users.DTO;
using MediatR;

namespace HrApp.Application.Users.Query.GetUserById;

public class GetUserByIdQuery(Guid UserId) : IRequest<UserDTO?>
{
    public Guid UserId { get; set; } = UserId;
}
