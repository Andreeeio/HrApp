using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class WorkedHoursRaportConfiguration : IEntityTypeConfiguration<WorkedHoursRaport>
{
    public void Configure(EntityTypeBuilder<WorkedHoursRaport> builder)
    {
        builder.HasOne(w => w.User)
            .WithMany(u => u.WorkedHoursRaports)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
