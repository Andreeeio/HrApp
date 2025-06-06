using HrApp.Application.Interfaces;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Services;

public class SalaryHistoryGenerator : ISalaryHistoryGenerator
{
    private readonly ILogger<SalaryHistoryGenerator> _logger;
    private readonly ISalaryRepository _salaryRepository;
    private readonly IEmployeeRateRepository _employeeRateRepository;
    private readonly ISalaryHistoryRepository _salaryHistoryRepository;

    public SalaryHistoryGenerator(
        ISalaryRepository salaryRepository,
        IEmployeeRateRepository employeeRateRepository,
        ISalaryHistoryRepository salaryHistoryRepository,
        ILogger<SalaryHistoryGenerator> logger)
    {
        _logger = logger;
        _salaryRepository = salaryRepository;
        _employeeRateRepository = employeeRateRepository;
        _salaryHistoryRepository = salaryHistoryRepository;
    }

    public async Task GenerateSalaryHistoryAsync()
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);
        var paidList = await _salaryRepository.GetAllPaidAsync();

        foreach (var paid in paidList)
        {
            var rates = await _employeeRateRepository.GetRatesForUserAsync(paid.UserId);
            if (!rates.Any())
                rates.Add(new HrApp.Domain.Entities.EmployeeRate
                {
                    EmployeeId = paid.UserId,
                    Rate = 3 
                });

            var averageRate = rates.Average(r => r.Rate);
            float modifier = averageRate switch
            {
                <= 1 => 0.9f,
                2 => 0.95f,
                3 => 1.0f,
                4 => 1.05f,
                5 => 1.10f,
                _ => 1.0f
            };

            var finalSalary = paid.BaseSalary * modifier;
            var salaryHistory = new SalaryHistory
            {
                Id = Guid.NewGuid(),
                UserId = paid.UserId,
                Salary = finalSalary,
                MonthNYear = new DateOnly(now.Year, now.Month, 1)
            };

            await _salaryHistoryRepository.AddAsync(salaryHistory);
            _logger.LogInformation("Salary history added for user {UserId}: {Salary}", paid.UserId, finalSalary);
        }
    }
}
