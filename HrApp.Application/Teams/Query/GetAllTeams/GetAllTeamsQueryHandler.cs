using AutoMapper;
using HrApp.Application.Interfaces;
using HrApp.Application.Teams.DTO;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Teams.Query.GetAllTeams;

public class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, List<TeamDTO>>
{
    private readonly ILogger<GetAllTeamsQueryHandler> _logger;
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public GetAllTeamsQueryHandler(ILogger<GetAllTeamsQueryHandler> logger,
        ITeamRepository teamRepository, 
        IMapper mapper)
    {
        _logger = logger;
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<List<TeamDTO>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all teams");

        var teams = await _teamRepository.GetAllTeamsAsync();
        var dto = _mapper.Map<List<TeamDTO>>(teams);

        return dto;
    }
}
