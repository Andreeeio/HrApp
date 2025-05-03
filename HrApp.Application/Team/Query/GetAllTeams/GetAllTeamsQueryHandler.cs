using HrApp.Application.Team.DTO;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Team.Query.GetAllTeams;

public class GetAllTeamsQueryHandler(ILogger<GetAllTeamsQueryHandler> logger) : IRequestHandler<GetAllTeamsQuery, List<TeamDTO>>
{
    public Task<List<TeamDTO>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        
        throw new NotImplementedException();
    }
}
