using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class LeaderFeedbackConfiguration : IEntityTypeConfiguration<LeaderFeedback>
{
    public void Configure(EntityTypeBuilder<LeaderFeedback> builder)
    {
        builder.HasOne(e => e.Assignment)
            .WithMany(u => u.LeaderFeedbacks)
            .HasForeignKey(e => e.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.Feedback)
            .IsRequired()
            .HasMaxLength(150);
    }
}
