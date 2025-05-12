using MediatR;

namespace HrApp.Application.Teams.Command.AddEmployer;

public class AddEmployerCommand : IRequest
{
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
}
