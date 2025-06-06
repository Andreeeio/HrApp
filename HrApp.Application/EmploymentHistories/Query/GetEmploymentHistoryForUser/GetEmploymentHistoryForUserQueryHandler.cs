using AutoMapper;
using HrApp.Application.EmploymentHistories.DTO;
using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.EmploymentHistories.Query.GetEmploymentHistoryForUser;

public class GetEmploymentHistoryForUserQueryHandler: IRequestHandler<GetEmploymentHistoryForUserQuery, List<EmploymentHistoryDTO>>
{
    private readonly ILogger<GetEmploymentHistoryForUserQueryHandler> _logger;
    private readonly IEmploymentHistoryRepository _employmentHistoryRepository;
    private readonly IUserContext _userContext;
    public readonly IUserRepository _userRepository;
    private readonly ITeamAuthorizationService _teamAuthorizationService;
    private readonly IMapper _mapper;

    public GetEmploymentHistoryForUserQueryHandler(ILogger<GetEmploymentHistoryForUserQueryHandler> logger,
        IEmploymentHistoryRepository employmentHistoryRepository,
        IUserContext userContext,
        IUserRepository userRepository,
        ITeamAuthorizationService teamAuthorizationService,
        IMapper mapper)
    {
        _logger = logger;
        _employmentHistoryRepository = employmentHistoryRepository;
        _userContext = userContext;
        _userRepository = userRepository;
        _teamAuthorizationService = teamAuthorizationService;
        _mapper = mapper;
    }

    public async Task<List<EmploymentHistoryDTO>> Handle(GetEmploymentHistoryForUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetEmploymentHistoryForUserQuery for user {UserId}", request.UserId);

        var user = _userContext.GetCurrentUser();

        if (user == null)
            throw new UnauthorizedException("User not authenticated");

        if (!(Guid.Parse(user.id) == request.UserId) && !(_teamAuthorizationService.Authorize(ResourceOperation.Read)))
            throw new UnauthorizedException("User notAuthorized");

        if(await _userRepository.IfUserExistAsync(request.UserId) == false)
            throw new BadRequestException("User not found");

        var employmentHistory = await _employmentHistoryRepository.GetEmploymentHistoriesForUserAsync(request.UserId);

        var employmentHistoryDTO = _mapper.Map<List<EmploymentHistoryDTO>>(employmentHistory);

        return employmentHistoryDTO;
    }
}
