using MediatR;

namespace HrApp.Application.Users.Command.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public Guid UserId { get; set; }
}