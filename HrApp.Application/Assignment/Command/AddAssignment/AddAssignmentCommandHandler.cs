using AutoMapper;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HrApp.Application.Assignment.Command.AddAssignment
{
    public class AddAssignmentCommandHandler : IRequestHandler<AddAssignmentCommand>
    {
        private readonly ILogger _logger;
        private readonly IAssignmentRepository _repositorya;
        private readonly IUserRepository _repositoryu;
        private readonly IMapper _mapper;

        public AddAssignmentCommandHandler(ILogger<AddAssignmentCommandHandler> logger, IAssignmentRepository repositorya, IUserRepository repositoryu, IMapper mapper)
        {
            _logger = logger;
            _repositorya = repositorya;
            _mapper = mapper;
            _repositoryu = repositoryu;
        }

        public async Task Handle(AddAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = _mapper.Map<Domain.Entities.Assignment>(request);
            _logger.LogInformation("Adding assignment {AssignmentName}", assignment.Name);
            await _repositorya.AddAssignment(assignment);

            return;
        }
    }
}
