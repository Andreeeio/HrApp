using AutoMapper;
using HrApp.Application.Interfaces;
using HrApp.Application.Teams.DTO;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Teams.Query.GetTeamForUser;

public class GetTeamForUserQueryHandler : IRequestHandler<GetTeamForUserQuery, TeamDTO>
{
    private readonly ILogger<GetTeamForUserQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly ITeamRepository _repository;
    private readonly IMapper _mapper;

    public GetTeamForUserQueryHandler(ILogger<GetTeamForUserQueryHandler> logger, IUserContext userContext, ITeamRepository repository, IMapper mapper) 
    {
        _logger = logger;
        _userContext = userContext;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TeamDTO> Handle(GetTeamForUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting team for user");

        Team? team;

        if (request.Id.HasValue)
        {
            team = await _repository.GetTeamForUserAsync(request.Id.Value);

        }
        else
        {
            var user = _userContext.GetCurrentUser();
            if (user == null)
                throw new UnauthorizedException("User is not authenticated");
            team = await _repository.GetTeamForUserAsync(Guid.Parse(user.id));

        }

        if (team == null)
        {
            throw new BadRequestException("Team has not been found");
        }
        var dto = _mapper.Map<TeamDTO>(team);

        return dto;
    }
}