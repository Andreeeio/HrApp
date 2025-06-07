using HrApp.Application.Interfaces;
using HrApp.Application.Users.DTO;
using HrApp.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Users.Query.GetDataFromToken;

public class GetDataFromTokenQueryHandler(ILogger<GetDataFromTokenQueryHandler> logger,
    IUserContext userContext) : IRequestHandler<GetDataFromTokenQuery,CurrentUser?>
{
    private readonly ILogger<GetDataFromTokenQueryHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;
    public async Task<CurrentUser?> Handle(GetDataFromTokenQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User data");

        var currentUser = _userContext.GetCurrentUser()
            ?? throw new UnauthorizedException(nameof(CurrentUser));

        var user = await Task.FromResult(currentUser);

        return user;
    }
}
