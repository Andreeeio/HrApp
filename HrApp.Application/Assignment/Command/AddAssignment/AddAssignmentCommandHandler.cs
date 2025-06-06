using AutoMapper;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Assignment.Command.AddAssignment;

public class AddAssignmentCommandHandler : IRequestHandler<AddAssignmentCommand>
{
    private readonly ILogger _logger;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;

    public AddAssignmentCommandHandler(ILogger<AddAssignmentCommandHandler> logger, IAssignmentRepository assignmentRepository, IMapper mapper)
    {
        _logger = logger;
        _assignmentRepository = assignmentRepository;
        _mapper = mapper;
    }

    public async Task Handle(AddAssignmentCommand request, CancellationToken cancellationToken)
    {
        var assignment = _mapper.Map<Domain.Entities.Assignment>(request);
        _logger.LogInformation("Adding assignment {AssignmentName}", assignment.Name);
        await _assignmentRepository.AddAssignmentAsync(assignment);
    }
}
