using MediatR;
using System;

namespace HrApp.Application.Users.Command.EditUser;

public class EditUserCommand : IRequest
{
    public Guid Id { get; set; } 
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
