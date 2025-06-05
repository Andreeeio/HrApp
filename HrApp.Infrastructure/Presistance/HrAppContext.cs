using HrApp.Domain.Entities;
using HrApp.Infrastructure.Presistance.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HrApp.Infrastructure.Presistance;

public class HrAppContext : DbContext
{
    public HrAppContext(DbContextOptions<HrAppContext> options) : base(options)
    {
    }

    public DbSet<AnonymousFeedback> AnonymousFeedback { get; set; } 
    public DbSet<Assignment> Assignment { get; set; } 
    public DbSet<AssignmentNotification> AssignmentNotification { get; set; }
    public DbSet<AssignmentRaport> AssignmentRaport { get; set; }
    public DbSet<Authorization> Authorization { get; set; } 
    public DbSet<Calendar> Calendar { get; set; } 
    public DbSet<CalendarEventCreator> CalendarEventCreator { get; set; }
    public DbSet<Candidate> Candidate { get; set; } 
    public DbSet<Department> Department { get; set; } 
    public DbSet<EmployeeRate> EmployeeRate { get; set; }
    public DbSet<EmploymentHistory> EmploymentHistory { get; set; }
    public DbSet<ExcelImport> ExcelImport { get; set; }
    public DbSet<GoogleOAuthToken> GoogleOAuthToken { get; set; }
    public DbSet<JobApplication> JobApplication { get; set; }
    public DbSet<LeaderFeedback> LeaderFeedback { get; set; } 
    public DbSet<Leave> Leave { get; set; } 
    public DbSet<Offer> Offer { get; set; }
    public DbSet<OverallRaport> OverallRaport { get; set; }
    public DbSet<Paid> Paid { get; set; } 
    public DbSet<Role> Role { get; set; } 
    public DbSet<SalaryHistory> SalaryHistory { get; set; } 
    public DbSet<Team> Team { get; set; } 
    public DbSet<TeamRaport> TeamRaport { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<UserIpAddress> UserIpAddress { get; set; }
    public DbSet<UserRaport> UserRaport { get; set; }
    public DbSet<WorkedHoursRaport> WorkedHoursRaport { get; set; }
    public DbSet<WorkLog> WorkLog { get; set; }
    public DbSet<WorkLogExportHistory> WorkLogExportHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AnonymousFeedbackConfiguration());
        modelBuilder.ApplyConfiguration(new JobApplicationConfiguration());
        modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
        modelBuilder.ApplyConfiguration(new AssignmentNotificationConfiguration());
        modelBuilder.ApplyConfiguration(new AuthorizationConfiguration());
        modelBuilder.ApplyConfiguration(new CalendarConfiguration()); 
        modelBuilder.ApplyConfiguration(new CalendarEventCreatorConfiguration());
        modelBuilder.ApplyConfiguration(new CandidateConfiguration());
        modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeRateConfiguration());
        modelBuilder.ApplyConfiguration(new EmploymentHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new ExellImportConfiguration());
        modelBuilder.ApplyConfiguration(new GoogleOAuthTokenConfiguration());
        modelBuilder.ApplyConfiguration(new LeaderFeedbackConfiguration());
        modelBuilder.ApplyConfiguration(new LeaveConfiguration());
        modelBuilder.ApplyConfiguration(new OfferConfiguration());
        modelBuilder.ApplyConfiguration(new PaidConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new SalaryHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserIpAddressConfiguration());
        modelBuilder.ApplyConfiguration(new WorkedHoursRaportConfiguration());
        modelBuilder.ApplyConfiguration(new WorkLogConfiguration());
        modelBuilder.ApplyConfiguration(new WorkLogExportHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new UserRaportConfiguration());
        modelBuilder.ApplyConfiguration(new TeamRaportConfiguration());
        modelBuilder.ApplyConfiguration(new AssignmentRaportConfiguration());
        modelBuilder.ApplyConfiguration(new OverallRaportConfiguration());


        modelBuilder.Entity<User>()
            .HasOne(u => u.TeamLeader)
            .WithMany()
            .HasForeignKey(u => u.TeamLeaderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
