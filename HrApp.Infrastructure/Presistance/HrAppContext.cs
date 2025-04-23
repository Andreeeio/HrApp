using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Presistance;

public class HrAppContext : DbContext
{
    public HrAppContext(DbContextOptions<HrAppContext> options) : base(options)
    {
    }

    public DbSet<AnonymousFeedback> AnonymousFeedbacks { get; set; } 
    public DbSet<Application> Application { get; set; } 
    public DbSet<Assignment> Assignment { get; set; } 
    public DbSet<AssignmentNotification> AssignmentNotification { get; set; }
    public DbSet<Authorization> Authorization { get; set; } 
    public DbSet<Calendar> Calendar { get; set; } 
    public DbSet<Candidate> Candidate { get; set; } 
    public DbSet<Department> Department { get; set; } 
    public DbSet<EmployeeRate> EmployeeRate { get; set; }
    public DbSet<EmploymentHistory> EmploymentHistory { get; set; }
    public DbSet<ExellImport> ExellImports { get; set; } 
    public DbSet<LeaderFeedback> LeaderFeedback { get; set; } 
    public DbSet<Leave> Leave { get; set; } 
    public DbSet<Offer> Offer { get; set; }
    public DbSet<Paid> Paid { get; set; } 
    public DbSet<Role> Role { get; set; } 
    public DbSet<SalaryHistory> SalaryHistory { get; set; } 
    public DbSet<Team> Team { get; set; } 
    public DbSet<User> User { get; set; } 
    public DbSet<WorkedHoursRaport> WorkedHoursRaport { get; set; }
    public DbSet<WorkLog> WorkLog { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

    }
}
