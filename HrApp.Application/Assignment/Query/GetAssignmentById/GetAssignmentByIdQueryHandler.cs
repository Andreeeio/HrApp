using AutoMapper;
using HrApp.Application.Assignment.Command.EditAssignment;
using HrApp.Application.Assignment.DTO;
using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Assignment.Query.GetAssignmentById;

public class GetAssignmentByIdQueryHandler(ILogger<GetAssignmentByIdQueryHandler> logger,
    IAssignmentRepository assignmentRepository,
    IUserContext userContext,
    IUserAuthorizationService userAuthorizationService,
    IMapper mapper) : IRequestHandler<GetAssignmentByIdQuery, EditAssignmentCommand>
{
    private readonly ILogger<GetAssignmentByIdQueryHandler> _logger = logger;
    private readonly IAssignmentRepository _assignmentRepository = assignmentRepository;
    private readonly IUserContext _userContext = userContext;
    private readonly IUserAuthorizationService _userAuthorizationService = userAuthorizationService;
    private readonly IMapper _mapper = mapper;
    public async Task<EditAssignmentCommand> Handle(GetAssignmentByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Editing assignment with ID: {Id}", request.Id);

        if (!_userAuthorizationService.Authorize(ResourceOperation.Update))
        {
            _logger.LogWarning("User is not authorized to edit assignment with ID: {Id}", request.Id);
            throw new UnauthorizedException("User is not authorized to edit this assignment.");
        }

        var assignment = await _assignmentRepository.GetAssignmentByIdAsync(request.Id);

        if (assignment == null)
            throw new BadRequestException($"Assignment with ID: {request.Id} not found");

        var assignmentDto = _mapper.Map<EditAssignmentCommand>(assignment);

        return assignmentDto;
    }
}
