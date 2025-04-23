using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.HasOne(a => a.AssignedToTeam)
            .WithMany(t => t.Assignments)
            .HasForeignKey(a => a.AssignedToTeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.LeaderFeedback)
            .WithOne()  
            .HasForeignKey<Assignment>(a => a.LeaderFeedbackId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(a => a.AssignmentNotifications)
            .WithOne(a => a.Assignment)
            .HasForeignKey(an => an.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(60); 

        builder.Property(a => a.Description)
            .HasMaxLength(300);
    }
}
