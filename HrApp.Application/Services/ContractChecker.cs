using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Text;

namespace HrApp.Application.Services;

public class ContractChecker : IContractChecker
{
    private readonly ILogger<ContractChecker> _logger;
    private readonly IEmploymentHistoryRepository _employmentHistoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;

    public ContractChecker(ILogger<ContractChecker> logger,
        IEmploymentHistoryRepository employmentHistoryRepository,
        IUserRepository userRepository,
        IEmailSender emailSender)
    {
        _logger = logger;
        _employmentHistoryRepository = employmentHistoryRepository;
        _userRepository = userRepository;
        _emailSender = emailSender;
    }

    public async Task Check()
    {
        _logger.LogInformation("Handling Expiring contracts");

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateFrom = today.AddDays(30);

        var empHist = await _employmentHistoryRepository.GetExpiringContractsAsync(today, dateFrom);

        List<string> roles = new()
        {
            Roles.Hr.ToString(),
            Roles.Ceo.ToString()
        };

        var hr = await _userRepository.GetUserWithRolesAsync(roles);

        StringBuilder sb = new();

        foreach (var emp in empHist)
        {
            var user = await _userRepository.GetUserAsync(emp.UserId);
            await _emailSender.SendEmailAsync(user!.Email, "Contract Expiration Reminder",
                $"Dear {user.FirstName},\n\n" +
                $"Your contract is expiring on {emp.EndDate:dd/MM/yyyy}.\n" +
                $"Please contact HR for further assistance.\n\n" +
                $"Best regards,\nYour HR Team");

            sb.AppendLine($"User: {user.FirstName} {user.LastName}, with email: {user.Email}, Contract End Date: {emp.EndDate:dd/MM/yyyy}\n");
        }

        if (sb.Length == 0)
        {
            foreach (var hrUser in hr)
            {
                await _emailSender.SendEmailAsync(hrUser.Email, "Contract Expiration Notification",
                    $"Dear {hrUser.FirstName},\n\n" +
                    $"There is no expiring contracts\n" +
                    $"Best regards,\nYour HR Team");
            }
        }
        else
        {
            foreach (var hrUser in hr)
            {
                await _emailSender.SendEmailAsync(hrUser.Email, "Contract Expiration Notification",
                    $"Dear {hrUser.FirstName},\n\n" +
                    $"The following contracts are expiring soon:\n{sb.ToString()}\n" +
                    $"Please take necessary actions.\n\n" +
                    $"Best regards,\nYour HR Team");
            }
        }
    }
}
