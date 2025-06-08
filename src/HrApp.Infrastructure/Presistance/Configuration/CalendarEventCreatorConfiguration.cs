using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class CalendarEventCreatorConfiguration : IEntityTypeConfiguration<CalendarEventCreator>
{
    public void Configure(EntityTypeBuilder<CalendarEventCreator> builder)
    {
        builder.HasOne(cec => cec.Calendar)
            .WithOne(c => c.Creator)
            .HasForeignKey<CalendarEventCreator>(cec => cec.CalendarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
