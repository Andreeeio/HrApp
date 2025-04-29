using System.Data;
using System.Security.Cryptography;
using System.Text;
using Bogus;
using HrApp.Domain.Constants;
using HrApp.Domain.Entities;
using HrApp.Infrastructure.Presistance;

namespace HrApp.Infrastructure.Seeder;

public class HrAppSeeder(HrAppContext dbContext) : IHrAppSeeder
{
    private readonly HrAppContext _dbContext = dbContext;
    private const string Locale = "pl";

    public async Task Seeder()
    {
        if (!_dbContext.Role.Any())
        {
            var roles = GetRoles();
            _dbContext.AddRange(roles);
            await _dbContext.SaveChangesAsync();
        }

        if(!_dbContext.User.Any())
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
                    users[i].Roles.Add(role.FirstOrDefault(x => x.Name == Roles.Ceo.ToString())!);
                else if(i == 1)
                    users[i].Roles.Add(role.FirstOrDefault(x => x.Name == Roles.TeamLeader.ToString())!);
                else if (i == 2)
                    users[i].Roles.Add(role.FirstOrDefault(x => x.Name == Roles.Hr.ToString())!);
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

                users[i].Leaves.Add(leaves[i]);
                users[i].EmploymentHistories.Add(empHist[i]);
                users[i].WorkLogs = workLog;
                users[i].Paid = paids[i];
                rates[i].RatedBy = users[0];
                users[i].EmployeeRates.Add(rates[i]);

                _dbContext.Leave.AddRange(leaves);
                _dbContext.EmploymentHistory.AddRange(empHist);
                _dbContext.WorkLog.AddRange(workLog);
                _dbContext.Paid.AddRange(paids);
                _dbContext.EmployeeRate.AddRange(rates);
            }
            _dbContext.User.AddRange(users);
            await _dbContext.SaveChangesAsync();

        }

        if(!_dbContext.Team.Any())
        {

            Team team = new Team();
        }
    }

    private List<Role> GetRoles()
    {
        return Enum.GetNames(typeof(Role))
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
            .RuleFor(x => x.StartDate, y => y.Date.FutureDateOnly())
            .RuleFor(x => x.EndDate, (y, x) => x.StartDate.AddMonths(y.Random.Int(1, 10)))
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


}
