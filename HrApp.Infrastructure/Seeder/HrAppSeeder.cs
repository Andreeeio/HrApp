using System.Security.Cryptography;
using System.Text;
using Bogus;
using HrApp.Domain.Entities;
using HrApp.Infrastructure.Presistance;

namespace HrApp.Infrastructure.Seeder;

public class HrAppSeeder(HrAppContext dbContext) : IHrAppSeeder
{
    private readonly HrAppContext _dbContext = dbContext;
    private const string Locale = "pl";

    public async Task Seeder()
    {
        if (_dbContext.Role.Any())
        {
            var roles = GetRoles();
            _dbContext.AddRange(roles);
            await _dbContext.SaveChangesAsync();
        }

        if(_dbContext.User.Any())
        {
            var users = GetUsers();

            var roles = _dbContext.Role.ToArray();
            var leaves = GetLeaves();

            for(int i = 0; i < users.Count; i++)
            {
                users[i].Leaves.Add(leaves[i]);
                
            }

        }
        throw new NotImplementedException();
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
            .RuleFor(x => x.StartTime, y => y.Date.Between(new DateTime(2025,4,14)))
    }
}
