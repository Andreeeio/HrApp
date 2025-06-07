using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class CalendarConfiguration : IEntityTypeConfiguration<Calendar>
{
    public void Configure(EntityTypeBuilder<Calendar> builder)
    {
        builder.Property(c => c.Title)
            .IsRequired() 
            .HasMaxLength(60); 

        builder.Property(c => c.Description)
            .IsRequired() 
            .HasMaxLength(200);

        builder.HasOne(c => c.Creator)
            .WithOne(cec => cec.Calendar)
            .HasForeignKey<CalendarEventCreator>(cec => cec.CalendarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}