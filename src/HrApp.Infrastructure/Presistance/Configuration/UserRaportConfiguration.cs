using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class UserRaportConfiguration : IEntityTypeConfiguration<UserRaport>
{
    public void Configure(EntityTypeBuilder<UserRaport> builder)
    {
        builder.HasOne(u => u.OverallRaport)
            .WithMany(o => o.UserRaport)
            .HasForeignKey(u => u.OverallRaportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
