using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class AssignmentNotificationConfiguration : IEntityTypeConfiguration<AssignmentNotification>
{
    public void Configure(EntityTypeBuilder<AssignmentNotification> builder)
    {
        builder.HasOne(a => a.Assignment)
            .WithMany(a => a.AssignmentNotifications)
            .HasForeignKey(an => an.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(an => an.NotificationMessage)
            .IsRequired()
            .HasMaxLength(300); 

        builder.Property(an => an.MessageType)
            .IsRequired()
            .HasMaxLength(50);
    }
}