using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasMany(o => o.Applications)
            .WithOne(a => a.Offer)
            .HasForeignKey(a => a.OfferID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(o => o.PositionName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(o => o.Descritpion)
            .IsRequired()
            .HasMaxLength(200);
    }
}
