using AutoMapper;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Teams.Command.AddTeam
{
    public class AddTeamCommandHandler : IRequestHandler<AddTeamCommand>
    {
        private readonly ITeamRepository _repository;
        private readonly ILogger<AddTeamCommandHandler> _logger;
        private readonly IMapper _mapper;
        public AddTeamCommandHandler(ITeamRepository repository, ILogger<AddTeamCommandHandler> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task Handle(AddTeamCommand request, CancellationToken cancellationToken)
        {
            var team = _mapper.Map<Domain.Entities.Team>(request);
            team.DepartmentId = Guid.Parse(team.DepartmentId.ToString().ToUpper());
            await _repository.CreateTeam(team);
            _logger.LogInformation($"Added team {team.Name} to department {team.DepartmentId}");
            return;
        }
    }
}
