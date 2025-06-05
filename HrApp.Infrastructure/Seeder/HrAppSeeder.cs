using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Bogus;
using HrApp.Domain.Constants;
using HrApp.Domain.Entities;
using HrApp.Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Seeder;

public class HrAppSeeder(HrAppContext dbContext) : IHrAppSeeder
{
    private readonly HrAppContext _dbContext = dbContext;
    private const string Locale = "pl";

    public async Task Seed()
    {
        if (!_dbContext.Role.Any())
        {
            var roles = GetRoles();
            _dbContext.AddRange(roles);
            await _dbContext.SaveChangesAsync();
        }

        if (!_dbContext.User.Any())
        {
            var users = GetUsers();

            var role = _dbContext.Role.ToArray();
            var leaves = GetLeaves();
            var empHist = GetEmploymentsHistory();
            var workLog = GetWorkLog();
            var paids = GetPaids();
            var rates = GetRates();

            for (int i = 0; i < users.Count; i++)
            {
                if (i == 0)
                {
                    users[i].Roles = [role.FirstOrDefault(x => x.Name == Roles.Ceo.ToString())]; 
                }
                else if (i == 1)
                    users[i].Roles = [role.FirstOrDefault(x => x.Name == Roles.TeamLeader.ToString())!];
                else if (i == 2)
                    users[i].Roles = [role.FirstOrDefault(x => x.Name == Roles.Hr.ToString())!];
                else
                {
                    var excludedRoles = new[]
                    {
                        Roles.Ceo.ToString(),
                        Roles.TeamLeader.ToString(),
                        Roles.Hr.ToString()
                    };

                    var otherRoles = role.Where(x => !excludedRoles.Contains(x.Name)).ToList();
                    var roleIndex = Random.Shared.Next(otherRoles.Count());
                    users[i].Roles = [otherRoles[roleIndex]];
                }

                users[i].Leaves = [leaves[i]];
                empHist[i].Position = users[i].Roles[0].Name;
                users[i].EmploymentHistories = [empHist[i]];
                users[i].WorkLogs = workLog;
                users[i].Paid = paids[i];
                rates[i].RatedBy = users[0];
                users[i].EmployeeRates = [rates[i]];

                _dbContext.Leave.AddRange(leaves);
                _dbContext.EmploymentHistory.AddRange(empHist);
                _dbContext.WorkLog.AddRange(workLog);
                _dbContext.Paid.AddRange(paids);
                _dbContext.EmployeeRate.AddRange(rates);
            }
            _dbContext.User.AddRange(users);
            await _dbContext.SaveChangesAsync();

        }

        if (!_dbContext.Department.Any())
        {
            var department = GetDepartment();
            var users = _dbContext.User.Include(u => u.Roles).ToArray();
            var anonymousFeedbacks = GetAnonymousFeedbacks();
            var assignments = GetAssignments();
            var leaderFeedbacks = GetLeaderFeedbacks();
            var assignmentNotifications = GetAssignmentNotifications();

            for (int i = 0; i < leaderFeedbacks.Count; i++)
            {
                assignments[i].LeaderFeedbacks = [leaderFeedbacks[i]];
                assignments[i].AssignmentNotifications = [assignmentNotifications[i]];
            }

            Team team = new Team();
            team.TeamLeader = users.FirstOrDefault(x => x.Roles.Any(y => y.Name == Roles.TeamLeader.ToString()))!;
            team.Employers = users.Where(x => x.Roles.Any(y => y.Name == Roles.Senior.ToString() || y.Name == Roles.Mid.ToString() || y.Name == Roles.Junior.ToString())).ToList();
            team.AnonymousFeedbacks = anonymousFeedbacks;
            team.Assignments = assignments;
            team.Name = "Druzyna 1";
            department.Teams = [team];
            department.HeadOfDepartment = users.FirstOrDefault(x => x.Roles.Any(y => y.Name == Roles.Ceo.ToString()))!;

            _dbContext.Department.Add(department);
            _dbContext.AnonymousFeedbacks.AddRange(anonymousFeedbacks);
            _dbContext.Assignment.AddRange(assignments);
            _dbContext.LeaderFeedback.AddRange(leaderFeedbacks);
            _dbContext.AssignmentNotification.AddRange(assignmentNotifications);
            _dbContext.Team.Add(team);
            await _dbContext.SaveChangesAsync();
        }
        if (!_dbContext.Offer.Any())
        {
            var offers = GetOffers();
            var applications = GetApplications();
            var candidates = GetCandidates();
            var teams = _dbContext.Team.ToList(); 

            for (int i = 0; i < offers.Count; i++)
            {
                var team = teams[i % teams.Count];
                offers[i].Team = team;

                var application = applications[i];
                application.Candidate = candidates[i];

                offers[i].JobApplications = new List<JobApplication> { application };
            }

            _dbContext.Candidate.AddRange(candidates);
            _dbContext.Offer.AddRange(offers);
            _dbContext.JobApplication.AddRange(applications);
            await _dbContext.SaveChangesAsync();
        }

    }

    private List<Role> GetRoles()
    {
        return Enum.GetNames(typeof(Roles))
           .Select(name => new Role { Name = name })
           .ToList();
    }


    private List<User> GetUsers()
    {
        var (passwordHash, passwordSalt) = GeneratePassword("Password1");

        var users = new Faker<User>(Locale)
            .RuleFor(x => x.FirstName, y => y.Name.FirstName())
            .RuleFor(x => x.LastName, y => y.Name.LastName())
            .RuleFor(x => x.Email, y => y.Internet.Email())
            .RuleFor(x => x.IsEmailConfirmed, y => true)
            .RuleFor(x => x.DateOfBirth, y => y.Date.BetweenDateOnly(new DateOnly(1970, 1, 1), new DateOnly(2006, 12, 31)))
            .RuleFor(x => x.PasswordHash, passwordHash)
            .RuleFor(x => x.PasswordSalt, passwordSalt)
            .Generate(10);

        return users;
    }

    private (byte[] PasswordHash, byte[] PasswordSalt) GeneratePassword(string password)
    {
        using var hmac = new HMACSHA512();
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (passwordHash, hmac.Key);
    }

    private List<Leave> GetLeaves()
    {
        var leaves = new Faker<Leave>(Locale)
            .RuleFor(x => x.StartDate, y => y.Date.FutureDateOnly())
            .RuleFor(x => x.EndDate, (y, x) => x.StartDate.AddDays(y.Random.Int(1, 10)))
            .Generate(10);

        return leaves;
    }

    private List<EmploymentHistory> GetEmploymentsHistory()
    {
        var empHist = new Faker<EmploymentHistory>(Locale)
            .RuleFor(x => x.StartDate, y => y.Date.RecentDateOnly())
            .RuleFor(x => x.EndDate, (y, x) => x.StartDate.AddMonths(y.Random.Int(10, 20)))
            .Generate(10);

        return empHist;
    }

    private List<WorkLog> GetWorkLog()
    {
        var workLog = new Faker<WorkLog>(Locale)
            .RuleFor(x => x.StartTime, y => y.Date.Between(new DateTime(2025, 4, 1), new DateTime(2025, 4, 20)))
            .RuleFor(x => x.EndTime, (y, x) => x.StartTime.AddHours(y.Random.Int(1, 8)))
            .RuleFor(x => x.Hours, (y, x) => (int)(x.EndTime! - x.StartTime).Value.TotalHours)
            .Generate(5);

        return workLog;
    }

    private List<Paid> GetPaids()
    {
        var paids = new Faker<Paid>(Locale)
            .RuleFor(x => x.BaseSalary, y => y.Random.Float(3000, 10000))
            .Generate(10);

        return paids;
    }

    private List<EmployeeRate> GetRates()
    {
        var rates = new Faker<EmployeeRate>(Locale)
            .RuleFor(x => x.Rate, y => y.Random.Int(1, 5))
            .RuleFor(x => x.RateDate, y => y.Date.RecentDateOnly())
            .Generate(10);

        return rates;
    }

    private List<AnonymousFeedback> GetAnonymousFeedbacks()
    {
        var feedbacks = new Faker<AnonymousFeedback>(Locale)
            .RuleFor(x => x.Subject, y => y.Lorem.Sentence(3))
            .RuleFor(x => x.Message, y => y.Lorem.Sentence(5))
            .RuleFor(x => x.CreatedAt, y => y.Date.Recent())
            .Generate(10);

        return feedbacks;
    }

    private List<Assignment> GetAssignments()
    {
        var assignments = new Faker<Assignment>(Locale)
            .RuleFor(x => x.Name, y => y.Lorem.Sentence(5))
            .RuleFor(x => x.Description, y => y.Lorem.Sentence(3))
            .RuleFor(x => x.StartDate, y => y.Date.Past())
            .RuleFor(x => x.EndDate, y => y.Date.Recent().AddDays(30))
            .RuleFor(x => x.IsEnded, y => y.Random.Bool())
            .Generate(10);

        return assignments;
    }

    private List<LeaderFeedback> GetLeaderFeedbacks()
    {
        var feedbacks = new Faker<LeaderFeedback>(Locale)
            .RuleFor(x => x.Feedback, y => y.Lorem.Sentence(2))
            .RuleFor(x => x.CreatedAt, y => y.Date.Recent())
            .RuleFor(x => x.Rating, y => y.Random.Int(1, 5))
            .Generate(10);

        return feedbacks;
    }

    private List<AssignmentNotification> GetAssignmentNotifications()
    {
        var notifications = new Faker<AssignmentNotification>(Locale)
            .RuleFor(x => x.NotificationMessage, y => y.Lorem.Sentence(5))
            .RuleFor(x => x.MessageType, y => y.Lorem.Sentence(2))
            .RuleFor(x => x.SendDate, y => y.Date.Recent())
            .Generate(10);
     
        return notifications;
    }

    private Department GetDepartment()
    {
        var department = new Faker<Department>(Locale)
            .RuleFor(x => x.Name, y => y.Lorem.Sentence(2))
            .RuleFor(x => x.TeamTag, y => y.Lorem.Sentence(1))
            .Generate();

        return department;
    }

    private List<Offer> GetOffers()
    {
        var offers = new Faker<Offer>(Locale)
            .RuleFor(x => x.PositionName, y => y.Random.Word())
            .RuleFor(x => x.Salary, y => y.Random.Float(5000, 12000))
            .RuleFor(x => x.Description, y => y.Lorem.Sentence(5))
            .RuleFor(x => x.AddDate, y => y.Date.PastDateOnly())
            .Generate(10);

        return offers;
    }

    private List<JobApplication> GetApplications()
    {
        var applications = new Faker<JobApplication>(Locale)
            .RuleFor(x => x.ApplicationDate, y => y.Date.PastDateOnly())
            .RuleFor(x => x.Status, "Received")
            .RuleFor(x => x.CvLink, y => y.Internet.Url())
            .Generate(10);

        return applications;
    }

    private List<Candidate> GetCandidates()
    {
        var candidates = new Faker<Candidate>(Locale)
            .RuleFor(x => x.Name, y => y.Name.FirstName())
            .RuleFor(x => x.Surname, y => y.Name.LastName())
            .RuleFor(x => x.Email, y => y.Internet.Email())
            .RuleFor(x => x.HomeNumber, y => y.Random.Int())
            .RuleFor(x => x.Street, y => y.Lorem.Sentence(1))
            .RuleFor(x => x.City, y => y.Lorem.Sentence(1))
            .Generate(10);

        return candidates;
    }
    private List<WorkLogExportHistory> GetWorkLogExportHistory(List<User> users)
    {
        var faker = new Faker(Locale);
        var histories = new List<WorkLogExportHistory>();

        for (int i = 0; i < users.Count; i++)
        {
            var exportedBy = users[0]; 
            var exportedFor = users[i];

            var exportDate = faker.Date.Between(new DateTime(2025, 4, 1), new DateTime(2025, 4, 20));

            histories.Add(new WorkLogExportHistory
            {
                Id = Guid.NewGuid(),
                ExportedByUserId = exportedBy.Id,
                ExportedForUserId = exportedFor.Id,
                ExportDate = exportDate
            });
        }

        return histories;
    }
}