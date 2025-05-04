using HrApp.Application.Interfaces;
using HrApp.Application.Teams.DTO;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Teams.Query.GetAllTeams;

public class GetAllTeamsQueryHandler(ILogger<GetAllTeamsQueryHandler> logger,
    IUserContext userContext) : IRequestHandler<GetAllTeamsQuery, List<TeamDTO>>
{
    private readonly ILogger<GetAllTeamsQueryHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;
    public Task<List<TeamDTO>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all teams");
        var currentUser = _userContext.GetCurrentUser();
        throw new Exception("Not implemented yet");
    }
}
