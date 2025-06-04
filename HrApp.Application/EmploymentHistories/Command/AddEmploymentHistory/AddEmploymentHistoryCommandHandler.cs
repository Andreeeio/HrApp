using AutoMapper;
using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.EmploymentHistories.Command.AddEmploymentHistory;

public class AddEmploymentHistoryCommandHandler(ILogger<AddEmploymentHistoryCommandHandler> logger,
    IUserContext userContext,
    ITeamAuthorizationService teamAuthorizationService,
    IUserRepository userRepository,
    IEmploymentHistoryRepository employmentHistoryRepository,
    IMapper mapper) : IRequestHandler<AddEmploymentHistoryCommand>
{
    private readonly ILogger<AddEmploymentHistoryCommandHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;
    private readonly ITeamAuthorizationService _teamAuthorizationService = teamAuthorizationService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmploymentHistoryRepository _employmentHistoryRepository = employmentHistoryRepository;
    private readonly IMapper _mapper = mapper;
    public async Task Handle(AddEmploymentHistoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling AddEmploymentHistoryCommand for user {Email}", request.Email);

        var currentUser = _userContext.GetCurrentUser();

        if (currentUser == null)
            throw new UnauthorizedException("User is not authenticated.");
        
        if(!_teamAuthorizationService.Authorize(ResourceOperation.Create))
            throw new UnauthorizedException("User is not authorized to create employment history.");

        var user = await _userRepository.GetUserByEmail(request.Email);

        if (user == null)
            throw new BadRequestException($"User with email {request.Email} not found.");

        var lastEmployment = await _employmentHistoryRepository.GetLatestEmploymentHistoryForUserAsync(user.Id);

        if(lastEmployment != null)
        {
            lastEmployment.EndDate = request.StartDate.AddDays(-1);
            await _employmentHistoryRepository.SaveChangesAsync();
        }

        var empHistory = _mapper.Map<EmploymentHistory>(request);
        empHistory.UserId = user.Id;

        await _employmentHistoryRepository.AddEmploymentHistoryAsync(empHistory);
    }
}
