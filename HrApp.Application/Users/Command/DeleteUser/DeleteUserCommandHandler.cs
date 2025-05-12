using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Users.Command.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly ILogger<DeleteUserCommandHandler> _logger;
        private readonly IUserRepository _repository;
        public DeleteUserCommandHandler(ILogger<DeleteUserCommandHandler> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting user {UserId}", request.UserId);
            return _repository.DeleteUser(request.UserId);
        }
    }
}
