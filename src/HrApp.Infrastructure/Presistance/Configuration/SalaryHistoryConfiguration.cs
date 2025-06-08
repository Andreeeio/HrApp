using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class SalaryHistoryConfiguration : IEntityTypeConfiguration<SalaryHistory>
{
    public void Configure(EntityTypeBuilder<SalaryHistory> builder)
    {
        builder.HasOne(s => s.User)
            .WithMany(u => u.SalaryHistory)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
