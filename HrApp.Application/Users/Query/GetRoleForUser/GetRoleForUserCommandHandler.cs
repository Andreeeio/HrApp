using AutoMapper;
using HrApp.Application.Users.DTO;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Users.Query.GetRoleForUser;

public class GetRoleForUserCommandHandler(ILogger<GetRoleForUserCommandHandler> logger,
    IUserRepository userRepository,
    IMapper mapper) : IRequestHandler<GetRoleForUserCommand, List<string>>
{
    private readonly ILogger<GetRoleForUserCommandHandler> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
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
