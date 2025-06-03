using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Assignment.Command.EditAssignment;

public class EditAssignmentCommandHandler(ILogger<EditAssignmentCommandHandler> logger,
    IUserAuthorizationService userAuthorizationService,
    IAssignmentRepository assignmentRepository) : IRequestHandler<EditAssignmentCommand>
{
    private readonly ILogger<EditAssignmentCommandHandler> _logger = logger;
    private readonly IUserAuthorizationService _userAuthorizationService = userAuthorizationService;
    private readonly IAssignmentRepository _assignmentRepository = assignmentRepository;
    public async Task Handle(EditAssignmentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Editing assignment with ID: {Id}", request.Id);

        if(!_userAuthorizationService.Authorize(ResourceOperation.Update))
        {
            _logger.LogWarning("User is not authorized to edit assignment with ID: {Id}", request.Id);
            throw new UnauthorizedException("User is not authorized to edit this assignment.");
        }

        var assignment = await _assignmentRepository.GetAssignmentByIdAsync(request.Id);

        if(assignment == null)
            throw new BadRequestException($"Assignment with ID: {request.Id} not found");
        

        if (request.EndDate < DateTime.UtcNow)
            request.EndDate = DateTime.UtcNow.AddDays(1);

        if(request.EndDate > assignment.StartDate)
            request.EndDate = assignment.StartDate.AddDays(1);

        if (request.DifficultyLevel >= 5)
            request.DifficultyLevel = 5;

        assignment.EndDate = request.EndDate;
        assignment.DifficultyLevel = request.DifficultyLevel;

        await _assignmentRepository.SaveChangesAsync();
    }
}
