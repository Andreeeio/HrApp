using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Teams.Command.DeleteUserFromTeam
{
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
            await _repository.RemoveEmployer(request.TeamId, request.UserId);
            return;
        }
    }
}
