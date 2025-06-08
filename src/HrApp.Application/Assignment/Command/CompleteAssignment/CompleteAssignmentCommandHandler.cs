using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Assignment.Command.CompleteAssignment;

public class CompleteAssignmentCommandHandler : IRequestHandler<CompleteAssignmentCommand>
{
    private readonly ILogger<CompleteAssignmentCommandHandler> _logger;
    private readonly IAssignmentAuthorizationService _assignmentAuthorizationService;
    private readonly IAssignmentRepository _assignmentRepository;

    public CompleteAssignmentCommandHandler(ILogger<CompleteAssignmentCommandHandler> logger,
        IAssignmentAuthorizationService assignmentAuthorizationService,
        IAssignmentRepository assignmentRepository)
    {
        _logger = logger;
        _assignmentAuthorizationService = assignmentAuthorizationService;
        _assignmentRepository = assignmentRepository;
    }

    public async Task Handle(CompleteAssignmentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Ending a taks");

        var assignment = await _assignmentRepository.GetAssignmentByIdAsync(request.AssignmentId);

        if(assignment == null)
            throw new BadRequestException("Assignment not found");

        if(!await _assignmentAuthorizationService.Authorize(ResourceOperation.Update, assignment))
            throw new UnauthorizedAccessException("You are not authorized to complete this assignment");

        assignment.IsEnded = true;
        await _assignmentRepository.SaveChangesAsync();
    }
}
