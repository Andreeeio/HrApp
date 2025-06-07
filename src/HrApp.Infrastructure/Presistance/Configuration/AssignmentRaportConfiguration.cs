using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class AssignmentRaportConfiguration : IEntityTypeConfiguration<AssignmentRaport>
{
    public void Configure(EntityTypeBuilder<AssignmentRaport> builder)
    {
        builder.HasOne(a => a.OverallRaport)
            .WithMany(o => o.AssignmentRaport)
            .HasForeignKey(a => a.OverallRaportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
