using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class AnonymousFeedbackConfiguration : IEntityTypeConfiguration<AnonymousFeedback>
{
    public void Configure(EntityTypeBuilder<AnonymousFeedback> builder)
    {
        builder.HasOne(a => a.Team)
            .WithMany(t => t.AnonymousFeedbacks)
            .HasForeignKey(a => a.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(a => a.Subject)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Message)
            .IsRequired()
            .HasMaxLength(2000);

    }
}
