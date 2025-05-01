using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class EmployeeRateConfiguration : IEntityTypeConfiguration<EmployeeRate>
{
    public void Configure(EntityTypeBuilder<EmployeeRate> builder)
    {
        builder.HasOne(e => e.Employee)
            .WithMany(u => u.EmployeeRates)
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.RatedBy)
            .WithOne(u => u.Rater)
            .HasForeignKey<EmployeeRate>(e => e.RatedById)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
