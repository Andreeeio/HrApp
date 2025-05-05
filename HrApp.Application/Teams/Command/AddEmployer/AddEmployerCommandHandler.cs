using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Teams.Command.AddEmployer
{
    public class AddEmployerCommandHandler : IRequestHandler<AddEmployerCommand>
    {
        private readonly ITeamRepository _repository;
        private readonly ILogger<AddEmployerCommandHandler> _logger;

        public AddEmployerCommandHandler(ILogger<AddEmployerCommandHandler> logger,ITeamRepository repository)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(AddEmployerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding employer {UserId} to team {TeamId}", request.UserId, request.TeamId);
            await _repository.AddEmployer(request.TeamId, request.UserId);
            return;
        }
    }
}
