using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class TeamRaportConfiguration : IEntityTypeConfiguration<TeamRaport>
{
    public void Configure(EntityTypeBuilder<TeamRaport> builder)
    {
        builder.HasOne(t => t.OverallRaport)
            .WithMany(o => o.TeamRaport)
            .HasForeignKey(t => t.OverallRaportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
