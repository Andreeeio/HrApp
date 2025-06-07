using MediatR;

namespace HrApp.Application.Teams.Command.AddEmployer;

public class AddEmployerCommand : IRequest<Guid>
{
    public Guid TeamId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
}
