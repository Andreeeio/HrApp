using AutoMapper;
using HrApp.Application.Interfaces;
using HrApp.Application.SalaryHistories.DTO;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.SalaryHistories.Query.GetSalaryHistoryForUser;

public class GetSalaryHistoryForUserQueryHandler(ILogger<GetSalaryHistoryForUserQueryHandler> logger,
    IUserContext userContext,
    IUserAuthorizationService userAuthorizationService,
    ISalaryHistoryRepository salaryHistoryRepository,
    IMapper mapper) : IRequestHandler<GetSalaryHistoryForUserQuery, List<SalaryHistoryDTO>>
{
    private readonly IUserContext _userContext = userContext;
    private readonly ILogger<GetSalaryHistoryForUserQueryHandler> _logger = logger;
    private readonly IUserAuthorizationService _userAuthorizationService = userAuthorizationService;
    private readonly ISalaryHistoryRepository _salaryHistoryRepository = salaryHistoryRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<List<SalaryHistoryDTO>> Handle(GetSalaryHistoryForUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get salhistry for user");
        var user = _userContext.GetCurrentUser();

        if (user == null)
            throw new UnauthorizedException("User not found");

        if (!(Guid.Parse(user.id) == request.EmpId) && !(_userAuthorizationService.Authorize(ResourceOperation.Read)))
            throw new UnauthorizedException("User notAuthorized");

        if(request.HowMany == null || request.HowMany < 0)
            request.HowMany = 0;

        if(request.HowMany > 60)
            request.HowMany = 60;

        var salaryHistory = await _salaryHistoryRepository.GetSalaryHistoryForUser(request.EmpId, request.HowMany);

        var salaryHistoryDTO = _mapper.Map<List<SalaryHistoryDTO>>(salaryHistory);

        return salaryHistoryDTO;
    }
}
