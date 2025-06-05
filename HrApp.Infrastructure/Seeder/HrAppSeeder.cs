using System;
using System.Collections.Generic;
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
    private Task DisableConstraints(string tableName)
    {
        return _dbContext.Database.ExecuteSqlRawAsync($"ALTER TABLE [{tableName}] NOCHECK CONSTRAINT ALL;");
    }

    private Task EnableConstraints(string tableName)
    {
        return _dbContext.Database.ExecuteSqlRawAsync($"ALTER TABLE [{tableName}] WITH CHECK CHECK CONSTRAINT ALL;");
    }

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

            await DisableConstraints("Leave");
            await DisableConstraints("EmploymentHistory");
            await DisableConstraints("WorkLog");
            await DisableConstraints("Paid");
            await DisableConstraints("EmployeeRate");
            await DisableConstraints("RoleUser");
            await DisableConstraints("User");


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

            await EnableConstraints("EmploymentHistory");
            await EnableConstraints("WorkLog");
            await EnableConstraints("Paid");
            await EnableConstraints("EmployeeRate");
            await EnableConstraints("RoleUser");
            await EnableConstraints("User");
            await EnableConstraints("Leave");

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

            await DisableConstraints("AnonymousFeedback");
            await DisableConstraints("Assignment");
            await DisableConstraints("LeaderFeedback");
            await DisableConstraints("AssignmentNotification");
            await DisableConstraints("Team");
            await DisableConstraints("Department");
            await DisableConstraints("User");


            _dbContext.Department.Add(department);
            _dbContext.AnonymousFeedback.AddRange(anonymousFeedbacks);
            _dbContext.Assignment.AddRange(assignments);
            _dbContext.LeaderFeedback.AddRange(leaderFeedbacks);
            _dbContext.AssignmentNotification.AddRange(assignmentNotifications);
            _dbContext.Team.Add(team);
            await _dbContext.SaveChangesAsync();

            await EnableConstraints("AnonymousFeedback");
            await EnableConstraints("Assignment");
            await EnableConstraints("LeaderFeedback");
            await EnableConstraints("AssignmentNotification");
            await EnableConstraints("Team");
            await EnableConstraints("Department");
            await EnableConstraints("User");

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
            await DisableConstraints("Candidate");
            await DisableConstraints("JobApplication");
            await DisableConstraints("Offer");


            _dbContext.Candidate.AddRange(candidates);
            _dbContext.Offer.AddRange(offers);
            _dbContext.JobApplication.AddRange(applications);
            await _dbContext.SaveChangesAsync();

            await EnableConstraints("Candidate");
            await EnableConstraints("Offer");
            await EnableConstraints("JobApplication");

        }
        if (!_dbContext.AssignmentRaport.Any())
        {
            var assignments = _dbContext.Assignment.ToList();
            var teams = _dbContext.Team.ToList();
            var users = _dbContext.User.ToList();
            var overallRaports = _dbContext.OverallRaport.ToList();
            var authorizations = GetAuthorization(users);
            var creators = _dbContext.CalendarEventCreator.ToList();
            var calendars = GetCalendar();
            var assignmentRaports = GetAssignmentRaport(assignments, teams, overallRaports);
            var eventCreators = GetCalendarEventCreator(calendars);
            var excelImports = GetExcelImport(users);
            var googleOAuthTokens = GetGoogleOAuthToken(users);

            await DisableConstraints("Assignment");
            await DisableConstraints("AssignmentRaport");
            await DisableConstraints("Authorization");
            await DisableConstraints("Calendar");
            await DisableConstraints("CalendarEventCreator");
            await DisableConstraints("OverallRaport");
            await DisableConstraints("ExcelImport");
            await DisableConstraints("GoogleOAuthToken");

            _dbContext.AssignmentRaport.AddRange(assignmentRaports);
            _dbContext.Authorization.AddRange(authorizations);
            _dbContext.Calendar.AddRange(calendars);
            _dbContext.CalendarEventCreator.AddRange(eventCreators);
            _dbContext.ExcelImport.AddRange(excelImports);
            _dbContext.GoogleOAuthToken.AddRange(googleOAuthTokens);
            await _dbContext.SaveChangesAsync();

            await EnableConstraints("Assignment");
            await EnableConstraints("Authorization");
            await EnableConstraints("Calendar");
            await EnableConstraints("CalendarEventCreator");
            await EnableConstraints("OverallRaport");
            await EnableConstraints("AssignmentRaport");

            await EnableConstraints("ExcelImport");
            await EnableConstraints("GoogleOAuthToken");

        }
        if (!_dbContext.OverallRaport.Any())
        {
            var users = _dbContext.User.ToList();
            var teams = _dbContext.Team.ToList();
            var assignmentRaports = _dbContext.AssignmentRaport.ToList();
            var overallRaports = GetOverallRaport(assignmentRaports);
            var userRaports = GetUserRaport(users, teams, overallRaports);
            var teamLeaders = _dbContext.User.Where(u => u.Roles.Any(r => r.Name == Roles.TeamLeader.ToString())).ToList();
            var teamRaports = GetTeamRaport(teams, teamLeaders, overallRaports);
            var salaryHistories = GetSalaryHistory(users);
            var userIpAddresses = GetUserIpAddress(users);
            var workedHoursRaports = GetWorkedHoursRaport(users);
            var workLogExportHistory = GetWorkLogExportHistory(users);

            await DisableConstraints("SalaryHistory");
            await DisableConstraints("TeamRaport");
            await DisableConstraints("UserIpAddress");
            await DisableConstraints("UserRaport");
            await DisableConstraints("OverallRaport");

            await DisableConstraints("WorkedHoursRaport");
            await DisableConstraints("WorkLogExportHistory");

            _dbContext.OverallRaport.AddRange(overallRaports);
            _dbContext.SalaryHistory.AddRange(salaryHistories);
            _dbContext.TeamRaport.AddRange(teamRaports);
            _dbContext.UserIpAddress.AddRange(userIpAddresses);
            _dbContext.UserRaport.AddRange(userRaports);
            _dbContext.WorkedHoursRaport.AddRange(workedHoursRaports);
            _dbContext.WorkLogExportHistory.AddRange(workLogExportHistory);
            await _dbContext.SaveChangesAsync();

            await EnableConstraints("SalaryHistory");
            await EnableConstraints("UserIpAddress");
            await EnableConstraints("UserRaport");
            await EnableConstraints("OverallRaport");
            await EnableConstraints("TeamRaport");

            await EnableConstraints("WorkedHoursRaport");
            await EnableConstraints("WorkLogExportHistory");


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
            .RuleFor(x => x.IsEmailConfirmed, y => y.Random.Bool())
            .RuleFor(x => x.DateOfBirth, y => y.Date.BetweenDateOnly(new DateOnly(1970, 1, 1), new DateOnly(2006, 12, 31)))
            .RuleFor(x => x.PasswordHash, passwordHash)
            .RuleFor(x => x.PasswordSalt, passwordSalt)
            .Generate(500);

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
            .Generate(1000);

        return leaves;
    }

    private List<EmploymentHistory> GetEmploymentsHistory()
    {
        var empHist = new Faker<EmploymentHistory>(Locale)
            .RuleFor(x => x.StartDate, y => y.Date.RecentDateOnly())
            .RuleFor(x => x.EndDate, (y, x) => x.StartDate.AddMonths(y.Random.Int(10, 20)))
            .Generate(500);

        return empHist;
    }

    private List<WorkLog> GetWorkLog()
    {
        var workLog = new Faker<WorkLog>(Locale)
            .RuleFor(x => x.StartTime, y => y.Date.Between(new DateTime(2025, 4, 1), new DateTime(2025, 4, 20)))
            .RuleFor(x => x.EndTime, (y, x) => x.StartTime.AddHours(y.Random.Int(1, 8)))
            .RuleFor(x => x.Hours, (y, x) => (int)(x.EndTime! - x.StartTime).Value.TotalHours)
            .Generate(1500);

        return workLog;
    }

    private List<Paid> GetPaids()
    {
        var paids = new Faker<Paid>(Locale)
        .RuleFor(x => x.BaseSalary, y => (float)Math.Round(y.Random.Double(3000, 10000), 2))
            .Generate(500);

        return paids;
    }

    private List<EmployeeRate> GetRates()
    {
        var rates = new Faker<EmployeeRate>(Locale)
            .RuleFor(x => x.Rate, y => y.Random.Int(1, 5))
            .RuleFor(x => x.RateDate, y => y.Date.RecentDateOnly())
            .Generate(500);

        return rates;
    }

    private List<AnonymousFeedback> GetAnonymousFeedbacks()
    {
        var feedbacks = new Faker<AnonymousFeedback>(Locale)
            .RuleFor(x => x.Subject, y => y.Lorem.Sentence(3))
            .RuleFor(x => x.Message, y => y.Lorem.Sentence(5))
            .RuleFor(x => x.CreatedAt, y => y.Date.Recent())
            .Generate(400);

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
            .Generate(500);

        return assignments;
    }

    private List<LeaderFeedback> GetLeaderFeedbacks()
    {
        var feedbacks = new Faker<LeaderFeedback>(Locale)
            .RuleFor(x => x.Feedback, y => y.Lorem.Sentence(2))
            .RuleFor(x => x.CreatedAt, y => y.Date.Recent())
            .RuleFor(x => x.Rating, y => y.Random.Int(1, 5))
            .Generate(400);

        return feedbacks;
    }

    private List<AssignmentNotification> GetAssignmentNotifications()
    {
        var notifications = new Faker<AssignmentNotification>(Locale)
            .RuleFor(x => x.NotificationMessage, y => y.Lorem.Sentence(5))
            .RuleFor(x => x.MessageType, y => y.Lorem.Sentence(2))
            .RuleFor(x => x.SendDate, y => y.Date.Recent())
            .Generate(400);
     
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
            .Generate(100);

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
            .Generate(100);

        return candidates;
    }
    private List<AssignmentRaport> GetAssignmentRaport(List<Assignment> assignments, List<Team> teams, List<OverallRaport> overallRaports)
    {
        var assignmentRaports = new Faker<AssignmentRaport>(Locale)
            .RuleFor(x => x.AssignmentId, y => y.PickRandom(assignments).Id)
            .RuleFor(x => x.Name, y => y.Lorem.Word())
            .RuleFor(x => x.Description, y => y.Lorem.Sentence())
            .RuleFor(x => x.StartDate, y => y.Date.Past())
            .RuleFor(x => x.EndDate, (y, x) => x.StartDate.AddDays(y.Random.Int(1, 30)))
            .RuleFor(x => x.IsEnded, y => y.Random.Bool())
            .RuleFor(x => x.AssignedToTeamId, y => y.PickRandom(teams).Id)
            .RuleFor(x => x.DifficultyLevel, y => y.Random.Int(1, 5))
            .RuleFor(x => x.OverallRaportId, y => y.Random.Guid())
            .Generate(500);

        return assignmentRaports;
    }
    private List<Authorization> GetAuthorization(List<User> users)
    {
        var authorizationList = new Faker<Authorization>(Locale)
            .RuleFor(x => x.UserId, y => y.PickRandom(users).Id)
            .RuleFor(x => x.VerificationCode, y => y.Random.Int(1000, 9999))
            .RuleFor(x => x.VerificationCodeExpiration, y => y.Date.Future())
            .RuleFor(x => x.IsUsed, y => y.Random.Bool())
            .RuleFor(x => x.CreatedAt, y => y.Date.Past())
            .RuleFor(x => x.AttemptCount, y => y.Random.Int(0, 5))
            .RuleFor(x => x.IsActive, y => y.Random.Bool())
            .Generate(500);

        return authorizationList;
    }
    private List<Calendar> GetCalendar()
    {
        var calendarEvents = new Faker<Calendar>(Locale)
            .RuleFor(x => x.Title, y => y.Lorem.Sentence(3))
            .RuleFor(x => x.CreatedDate, y => y.Date.Past())
            .RuleFor(x => x.StartDate, y => y.Date.Future())
            .RuleFor(x => x.EndDate, (y, x) => x.StartDate.AddDays(y.Random.Int(1, 14)))
            .RuleFor(x => x.Description, y => y.Lorem.Paragraph())
            .Generate(1500);

        return calendarEvents;
    }
    private List<CalendarEventCreator> GetCalendarEventCreator(List<Calendar> calendars)
    {
        var eventCreators = new Faker<CalendarEventCreator>(Locale)
            .RuleFor(x => x.CalendarId, y => y.PickRandom(calendars).Id)
            .RuleFor(x => x.Email, y => y.Internet.Email())
            .Generate(1000);

        return eventCreators;
    }
    private List<ExcelImport> GetExcelImport(List<User> users)
    {
        var excelImports = new Faker<ExcelImport>(Locale)
            .RuleFor(x => x.UploadedById, y => y.PickRandom(users).Id)
            .RuleFor(x => x.FilePath, y => y.System.FilePath())
            .RuleFor(x => x.ImportDate, y => DateOnly.FromDateTime(y.Date.Past()))
            .Generate(500);

        return excelImports;
    }
    private List<GoogleOAuthToken> GetGoogleOAuthToken(List<User> users)
    {
        var googleOAuthTokens = new Faker<GoogleOAuthToken>(Locale)
            .RuleFor(x => x.UserId, y => y.PickRandom(users).Id)
            .RuleFor(x => x.AccessToken, y => y.Random.AlphaNumeric(50))
            .RuleFor(x => x.RefreshToken, y => y.Random.AlphaNumeric(50))
            .RuleFor(x => x.Expiry, y => y.Date.Future())
            .Generate(500);

        return googleOAuthTokens;
    }
    private List<OverallRaport> GetOverallRaport(List<AssignmentRaport> assignmentRaports)
    {
        var overallRaports = new Faker<OverallRaport>(Locale)
            .RuleFor(x => x.Name, y => y.Lorem.Sentence(3))
            .RuleFor(x => x.UserRaport, y => new List<UserRaport>())
            .RuleFor(x => x.TeamRaport, y => new List<TeamRaport>())
            .RuleFor(x => x.AssignmentRaport, y => y.PickRandom(assignmentRaports, y.Random.Int(1, assignmentRaports.Count)).ToList())
            .RuleFor(x => x.BackupDate, y => DateOnly.FromDateTime(y.Date.Past()))
            .Generate(500);

        return overallRaports;
    }
    private List<SalaryHistory> GetSalaryHistory(List<User> users)
    {
        var salaryHistories = new Faker<SalaryHistory>(Locale)
            .RuleFor(x => x.UserId, y => y.PickRandom(users).Id)
            .RuleFor(x => x.Salary, y => y.Random.Float(3000, 15000))
            .RuleFor(x => x.MonthNYear, y => DateOnly.FromDateTime(y.Date.Past()))
            .Generate(5000);

        return salaryHistories;
    }
    private List<TeamRaport> GetTeamRaport(List<Team> teams, List<User> teamLeaders, List<OverallRaport> overallRaports)
    {
        var teamRaports = new Faker<TeamRaport>(Locale)
            .RuleFor(x => x.TeamId, y => y.PickRandom(teams).Id)
            .RuleFor(x => x.TeamLeaderId, y => y.PickRandom(teamLeaders).Id)
            .RuleFor(x => x.Name, y => y.Lorem.Sentence(3))
            .RuleFor(x => x.OverallRaportId, y => y.PickRandom(overallRaports).Id)
            .Generate(600);

        return teamRaports;
    }
    private List<UserIpAddress> GetUserIpAddress(List<User> users)
    {
        return new Faker<UserIpAddress>(Locale)
            .RuleFor(x => x.UserId, y => y.PickRandom(users).Id)
            .RuleFor(x => x.IpAddress, y => y.Internet.Ip())
            .RuleFor(x => x.UserAgent, y => y.Internet.UserAgent())
            .RuleFor(x => x.LastAccessed, y => y.Date.Recent())
            .RuleFor(x => x.IsActive, y => y.Random.Bool())
            .Generate(500);
    }
    private List<UserRaport> GetUserRaport(List<User> users, List<Team> teams, List<OverallRaport> overallRaports)
    {
        return new Faker<UserRaport>(Locale)
            .RuleFor(x => x.UserId, y => y.PickRandom(users).Id)
            .RuleFor(x => x.FirstName, y => y.Name.FirstName())
            .RuleFor(x => x.LastName, y => y.Name.LastName())
            .RuleFor(x => x.Email, y => y.Internet.Email())
            .RuleFor(x => x.DateOfBirth, y => DateOnly.FromDateTime(y.Date.Past(30)))
            .RuleFor(x => x.YearRoundSalary, y => y.Random.Double(30000, 150000))
            .RuleFor(x => x.TeamId, y => y.PickRandom(teams).Id)
            .RuleFor(x => x.OverallRaportId, y => y.PickRandom(overallRaports).Id)
            .Generate(5000);
    }
    private List<WorkedHoursRaport> GetWorkedHoursRaport(List<User> users)
    {
        return new Faker<WorkedHoursRaport>(Locale)
            .RuleFor(x => x.UserId, y => y.PickRandom(users).Id)
            .RuleFor(x => x.WorkedHours, y => y.Random.Int(80, 200))
            .RuleFor(x => x.MonthNYear, y => DateOnly.FromDateTime(y.Date.Past()))
            .Generate(500);
    }
    private List<WorkLogExportHistory> GetWorkLogExportHistory(List<User> users)
    {
        var faker = new Faker(Locale);
        var histories = new List<WorkLogExportHistory>();

        foreach (var exportedFor in users)
        {
            for (int j = 0; j < 15; j++)
            {
                var exportedBy = faker.PickRandom(users);
                var exportDate = faker.Date.Between(new DateTime(2025, 4, 1), new DateTime(2025, 4, 20));

                histories.Add(new WorkLogExportHistory
                {
                    Id = Guid.NewGuid(),
                    ExportedByUserId = exportedBy.Id,
                    ExportedForUserId = exportedFor.Id,
                    ExportDate = exportDate
                });
            }
        }

        return histories;
    }
}