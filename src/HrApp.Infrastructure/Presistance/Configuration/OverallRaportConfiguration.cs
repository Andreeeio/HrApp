using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class OverallRaportConfiguration : IEntityTypeConfiguration<OverallRaport>
{
    public void Configure(EntityTypeBuilder<OverallRaport> builder)
    {
        builder.HasMany(o => o.UserRaport)
            .WithOne(u => u.OverallRaport)
            .HasForeignKey(u => u.OverallRaportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.TeamRaport)
            .WithOne(t => t.OverallRaport)
            .HasForeignKey(t => t.OverallRaportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.AssignmentRaport)
            .WithOne(a => a.OverallRaport)
            .HasForeignKey(a => a.OverallRaportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(o => o.Name)
            .HasMaxLength(30);
    }
}
