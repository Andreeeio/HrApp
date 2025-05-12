using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Teams.Command.DeleteTeam;

public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand>
{
    private readonly ILogger<DeleteTeamCommandHandler> _logger;
    private readonly ITeamRepository _teamRepository;
    public DeleteTeamCommandHandler(ILogger<DeleteTeamCommandHandler> logger,ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
        _logger = logger;
    }
    public async Task Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting team {TeamId}", request.TeamId);
        await _teamRepository.DeleteTeam(request.TeamId);
        return;
    }
}