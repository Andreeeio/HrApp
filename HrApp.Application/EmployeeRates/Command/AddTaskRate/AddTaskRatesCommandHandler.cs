using AutoMapper;
using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.EmployeeRates.Command.AddTaskRate;

public class AddTaskRatesCommandHandler(ILogger<AddTaskRatesCommandHandler> logger,
    IUserContext userContext,
    ITeamAuthorizationService teamAuthorizationService,
    IEmployeeRateRepository employeeRateRepository,
    IMapper mapper) : IRequestHandler<AddTaskRatesCommand>
{
    private readonly ILogger<AddTaskRatesCommandHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;
    private readonly ITeamAuthorizationService _teamAuthorizationService = teamAuthorizationService;
    private readonly IEmployeeRateRepository _employeeRateRepository = employeeRateRepository;
    private readonly IMapper _mapper = mapper;
    public async Task Handle(AddTaskRatesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding task rates for users in team");
        
        var user = _userContext.GetCurrentUser();

        if (user == null)
            throw new UnauthorizedException("User is not authorized");

        if (!_teamAuthorizationService.Authorize(ResourceOperation.Read))
            throw new UnauthorizedException("User is not authorized");

        var empRates = _mapper.Map<List<Domain.Entities.EmployeeRate>>(request.TaskRates);

        foreach (var empRate in empRates)
        {
            empRate.RatedById = Guid.Parse(user.id);
            empRate.RateDate = DateOnly.FromDateTime(DateTime.Now);
        }

        await _employeeRateRepository.AddRatesAsync(empRates);
    }
}
