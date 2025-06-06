using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Users.Command.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly IUserRepository _userRepository;
    public DeleteUserCommandHandler(ILogger<DeleteUserCommandHandler> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }
    public Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting user {UserId}", request.UserId);
        return _userRepository.DeleteUserAsync(request.UserId);
    }
}