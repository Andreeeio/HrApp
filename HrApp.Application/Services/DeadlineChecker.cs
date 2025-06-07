using HrApp.Application.Interfaces;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;

namespace HrApp.Application.Services;

public class DeadlineChecker(IEmailSender emailSender,
    IAssignmentRepository assignmentRepository,
    IUserRepository userRepository) : IDeadlineChecker
{
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IAssignmentRepository _assignmentRepository = assignmentRepository;
    IUserRepository _userRepository = userRepository;
    public async Task Check()
    {
        var assignments = await _assignmentRepository.GetActiveAssignmentsAsync();
        List<User> users;
        foreach (var assignment in assignments)
        {
            if (assignment.AssignedToTeamId is null)
                continue;

            users = await _userRepository.GetUserInTeamAsync(assignment.AssignedToTeamId);
            foreach(var user in users)
            {
                await _emailSender.SendEmailAsync(user.Email, "Deadline Reminder", 
                    $"Dear {user.FirstName}," +
                    $"\n\nThis is a reminder that your assignment '{assignment.Name}' is due tomorrow." +
                    $"\n\nBest regards,\nYour HR Team");
            }
        }
    }
}
