using AutoMapper;
using HrApp.Application.EmployeeRates.Command.AddTaskRate;
using HrApp.Application.EmployeeRates.Query.GetEmployeeToRate;
using HrApp.Application.Users.DTO;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.EmployeeRate.Query.GetEmployeeToRate;

public class GetEmployersToRateQueryHandler : IRequestHandler<GetEmployersToRateQuery, AddTaskRatesCommand>
{
    private readonly ILogger<GetEmployersToRateQueryHandler> _logger;
    private readonly ITeamAuthorizationService _teamAuthorizationService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetEmployersToRateQueryHandler(ILogger<GetEmployersToRateQueryHandler> logger,
        IUserRepository userRepository,
        ITeamAuthorizationService teamAuthorizationService,
        IMapper mapper)
    {
        _logger = logger;
        _teamAuthorizationService = teamAuthorizationService;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<AddTaskRatesCommand> Handle(GetEmployersToRateQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all employers in team {TeamId}", request.TeamId);

        if (!_teamAuthorizationService.Authorize(ResourceOperation.Read))
            throw new UnauthorizedException("User is not authorized");

        var users = await _userRepository.GetUserInTeamAsync(request.TeamId);
        var usersDTO = _mapper.Map<List<AddTaskRateCommand>>(users);

        AddTaskRatesCommand employees = new AddTaskRatesCommand();
        employees.TaskRates = usersDTO;

        return employees;
    }
}
