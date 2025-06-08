using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        // Relacja jeden do wielu z Team
        builder.HasOne(o => o.Team)
            .WithMany(t => t.Offers) // Dodaj właściwość Offers w Team, jeśli jej brakuje
            .HasForeignKey(o => o.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacja jeden do wielu z JobApplications
        builder.HasMany(o => o.JobApplications)
            .WithOne(a => a.Offer)
            .HasForeignKey(a => a.OfferID)
            .OnDelete(DeleteBehavior.Cascade);

        // Konfiguracja właściwości
        builder.Property(o => o.PositionName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(o => o.Description)
            .IsRequired()
            .HasMaxLength(200);
    }
}
