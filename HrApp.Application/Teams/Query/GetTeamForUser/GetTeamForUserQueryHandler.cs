using AutoMapper;
using HrApp.Application.Teams.DTO;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Teams.Query.GetTeamForUser;

public class GetTeamForUserQueryHandler : IRequestHandler<GetTeamForUserQuery, TeamDTO>
{
    private readonly ILogger<GetTeamForUserQueryHandler> _logger;
    private readonly ITeamRepository _repository;
    private readonly IMapper _mapper;

    public GetTeamForUserQueryHandler(ILogger<GetTeamForUserQueryHandler> logger, ITeamRepository repository, IMapper mapper) 
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<TeamDTO> Handle(GetTeamForUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting team for user");
        var team = await _repository.GetTeamForUser(request.UserId);
        var dto = _mapper.Map<TeamDTO>(team);

        return dto;
    }
}