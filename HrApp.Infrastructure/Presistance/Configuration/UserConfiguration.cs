using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users);

        builder.HasMany(u => u.Leaves)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.EmploymentHistories)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.WorkLogs)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Authorization)
            .WithOne(a => a.User)
            .HasForeignKey<Authorization>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.TeamLeader)
            .WithMany()
            .HasForeignKey(t => t.TeamLeaderId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(u => u.Team)
            .WithMany(t => t.Employers)
            .HasForeignKey(u => u.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.SalaryHistory)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Department)
            .WithOne(d => d.HeadOfDepartment)
            .HasForeignKey<Department>(d => d.HeadOfDepartmentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.WorkedHoursRaports)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Paid)
            .WithOne(p => p.User)
            .HasForeignKey<Paid>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ExellImports)
            .WithOne(e => e.UploadedBy)
            .HasForeignKey(e => e.UploadedById)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.EmployeeRates)
            .WithOne(er => er.Employee)
            .HasForeignKey(er => er.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Rater)
            .WithOne(er => er.RatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.GoogleOAuthTokens)
            .WithOne(g => g.User)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.IpAddresses)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}