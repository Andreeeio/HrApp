using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        // Relacja jeden do jednego z TeamLeader
        builder.HasOne(t => t.TeamLeader)
            .WithOne()
            .HasForeignKey<Team>(t => t.TeamLeaderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacja jeden do wielu z Employers
        builder.HasMany(t => t.Employers)
            .WithOne(tm => tm.Team)
            .HasForeignKey(tm => tm.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacja jeden do wielu z Offers
        builder.HasMany(t => t.Offers)
            .WithOne(o => o.Team)
            .HasForeignKey(o => o.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
