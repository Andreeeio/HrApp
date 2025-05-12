using HrApp.Application.Users.DTO;
using MediatR;

namespace HrApp.Application.Users.Query.GetUserByEmail;

public class GetUserByEmailQuery : IRequest<UserDTO>
{
    public string Email { get; set; } = string.Empty;
    public GetUserByEmailQuery(string email)
    {
        Email = email;
    }
}