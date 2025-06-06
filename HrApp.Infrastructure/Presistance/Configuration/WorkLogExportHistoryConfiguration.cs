using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class WorkLogExportHistoryConfiguration : IEntityTypeConfiguration<WorkLogExportHistory>
{
    public void Configure(EntityTypeBuilder<WorkLogExportHistory> builder)
    {
        builder.HasKey(e => e.Id);

        builder
            .HasOne(e => e.ExportedByUser)
            .WithMany(u => u.ExportedWorkLogs)
            .HasForeignKey(e => e.ExportedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.ExportedForUser)
            .WithMany(u => u.ReceivedExportedWorkLogs)
            .HasForeignKey(e => e.ExportedForUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
