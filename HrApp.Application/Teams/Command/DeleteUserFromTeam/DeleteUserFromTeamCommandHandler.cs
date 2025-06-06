﻿using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Teams.Command.DeleteUserFromTeam;

public class DeleteUserFromTeamCommandHandler : IRequestHandler<DeleteUserFromTeamCommand>
{
    private readonly ILogger<DeleteUserFromTeamCommandHandler> _logger;
    private readonly ITeamRepository _repository;

    public DeleteUserFromTeamCommandHandler(ILogger<DeleteUserFromTeamCommandHandler> logger,
        ITeamRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    public async Task Handle(DeleteUserFromTeamCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting employer {UserId} from team {TeamId}", request.UserId, request.TeamId);
        await _repository.RemoveEmployerAsync(request.TeamId, request.UserId);
        return;
    }
}