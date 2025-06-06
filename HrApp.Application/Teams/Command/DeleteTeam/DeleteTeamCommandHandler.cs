using HrApp.Domain.Exceptions;
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
        try
        {
            await _teamRepository.DeleteTeamAsync(request.TeamId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting team {TeamId}", request.TeamId);
            throw new BadRequestException("An error occurred while deleting the team.");
        }
    }
}