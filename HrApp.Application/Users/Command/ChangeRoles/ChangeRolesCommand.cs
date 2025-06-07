using MediatR;

namespace HrApp.Application.Users.Command.ChangeRoles;

public class ChangeRolesCommand : IRequest
{
    public ChangeRolesCommand(string email)
    {
        Email = email;
    }
    public string Email { get; set; } 
    public List<string> SelectedRoles { get; set; } = new();
}
