using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasOne(o => o.Team)
            .WithMany(t => t.Offers) 
            .HasForeignKey(o => o.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.JobApplications)
            .WithOne(a => a.Offer)
            .HasForeignKey(a => a.OfferID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(o => o.PositionName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(o => o.Description)
            .IsRequired()
            .HasMaxLength(200);
    }
}