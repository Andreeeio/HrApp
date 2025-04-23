using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.HasOne(a => a.Offer)
            .WithMany(o => o.Applications)
            .HasForeignKey(a => a.OfferID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Candidate)
            .WithOne(c => c.Application)
            .HasForeignKey<Application>(a => a.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.OfferID);
        builder.HasIndex(a => a.CandidateId);
    }
}
