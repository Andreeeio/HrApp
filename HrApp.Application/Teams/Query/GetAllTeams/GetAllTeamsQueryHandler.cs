using AutoMapper;
using HrApp.Application.Interfaces;
using HrApp.Application.Teams.DTO;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Teams.Query.GetAllTeams;

public class GetAllTeamsQueryHandler(ILogger<GetAllTeamsQueryHandler> logger,
    ITeamRepository repository, IMapper mapper) : IRequestHandler<GetAllTeamsQuery, List<TeamDTO>>
{
    private readonly ILogger<GetAllTeamsQueryHandler> _logger = logger;
    private readonly ITeamRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    public async Task<List<TeamDTO>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all teams");

        var teams = await _repository.GetAllTeamsAsync();
        var dto = _mapper.Map<List<TeamDTO>>(teams);

        return dto;
    }
}
