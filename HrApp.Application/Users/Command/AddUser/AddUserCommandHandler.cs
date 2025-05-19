using AutoMapper;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace HrApp.Application.Users.Command.AddUser;

public class AddUserCommandHandler(ILogger<AddUserCommandHandler> logger,
    IUserRepository userRepository,
    IMapper mapper) : IRequestHandler<AddUserCommand>
{
    private readonly ILogger<AddUserCommandHandler> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        if(await _userRepository.IfUserExist(request.Email))
        {
            throw new BadRequestException("User with this email already exists");
        }

        _logger.LogInformation("Creating new user");

        var hmac = new HMACSHA512();

        var user = _mapper.Map<User>(request);

        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
        user.PasswordSalt = hmac.Key;
        user.IsEmailConfirmed = false;
        user.ConfirmationTokenExpiration = DateTime.UtcNow.AddDays(1);
        user.ConfirmationToken = Guid.NewGuid().ToString();

        await _userRepository.CreateUser(user);
        _logger.LogInformation("New user created");

    }
}
