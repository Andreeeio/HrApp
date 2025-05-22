using HrApp.Application.Interfaces;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Services
{
    public class SalaryHistoryGenerator(
    ISalaryRepository salaryRepository,
    IEmployeeRateRepository rateRepository,
    ISalaryHistoryRepository salaryHistoryRepository,
    ILogger<SalaryHistoryGenerator> logger) : ISalaryHistoryGenerator
    {
        public async Task GenerateSalaryHistoryAsync()
        {
            Console.WriteLine("--------------------------------- Generationg salary ---------------------------------");
            var now = DateOnly.FromDateTime(DateTime.UtcNow);
            var paidList = await salaryRepository.GetAllPaidAsync();

            foreach (var paid in paidList)
            {
                var rates = await rateRepository.GetRatesForUserAsync(paid.UserId);
                if (!rates.Any())
                    rates.Add(new HrApp.Domain.Entities.EmployeeRate
                    {
                        EmployeeId = paid.UserId,
                        Rate = 3 // Default rate if none found
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

                await salaryHistoryRepository.AddAsync(salaryHistory);
                logger.LogInformation("Salary history added for user {UserId}: {Salary}", paid.UserId, finalSalary);
            }
        }
    }
}
