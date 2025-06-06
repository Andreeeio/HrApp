using AutoMapper;
using HrApp.Application.EmployeeRates.DTO;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.EmployeeRates.Query.GetRatesForUser;

public class GetRatesForUserQueryHandler : IRequestHandler<GetRatesForUserQuery, (List<EmployeeRateDto>, float)>
{
    private readonly ILogger<GetRatesForUserQueryHandler> _logger;
    private readonly IEmployeeRateRepository _employeeRateRepository;
    private readonly IMapper _mapper;

    public GetRatesForUserQueryHandler(
        ILogger<GetRatesForUserQueryHandler> logger,
        IEmployeeRateRepository employeeRateRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _logger = logger;
        _employeeRateRepository = employeeRateRepository;
        _mapper = mapper;
    }

    public async Task<(List<EmployeeRateDto>,float)> Handle(GetRatesForUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetRatesForUserQuery for user: {UserId}", request.UserId);

        var rates = await _employeeRateRepository.GetRatesForUserAsync(request.UserId);

        if (rates == null)
            throw new BadRequestException($"No rates found for user: {request.UserId}");
        
        var dto = _mapper.Map<List<EmployeeRateDto>>(rates);

        float totalRate = 0;

        foreach (var i in dto)
        {
            totalRate += i.Rate;
        }

        if (dto.Count != 0)
            totalRate /= dto.Count; 
        

        return (dto,totalRate);
    }
}
