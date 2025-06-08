using AutoMapper;
using HrApp.Application.Users.DTO;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Users.Query.GetRoleForUser;

public class GetRoleForUserCommandHandler : IRequestHandler<GetRoleForUserCommand, List<string>>
{
    private readonly ILogger<GetRoleForUserCommandHandler> _logger;
    private readonly IUserRepository _userRepository;

    public GetRoleForUserCommandHandler(ILogger<GetRoleForUserCommandHandler> logger,
        IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<List<string>> Handle(GetRoleForUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Geting roles for user");

        if(!await _userRepository.IfUserExistAsync(request.Email))
            throw new BadRequestException("User not found");

        var roles = await _userRepository.GetUserRolesAsync(request.Email);

        List<string> roleNames = new List<string>();

        foreach (var role in roles)
        {
            roleNames.Add(role.Name);
        }

        return roleNames;
    }
}
