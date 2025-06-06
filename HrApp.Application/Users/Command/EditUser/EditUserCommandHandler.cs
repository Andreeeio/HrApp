using AutoMapper;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Users.Command.EditUser;

public class EditUserCommandHandler : IRequestHandler<EditUserCommand>
{
    private readonly ILogger<EditUserCommandHandler> _logger;
    private readonly IUserRepository _userRepository;

    public EditUserCommandHandler(ILogger<EditUserCommandHandler> logger,
        IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(request.Id);
        if (user == null)
            throw new BadRequestException("User not found");

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;

        await _userRepository.SaveChangesAsync();
        _logger.LogInformation("User updated: {UserId}", request.Id);
    }
}
