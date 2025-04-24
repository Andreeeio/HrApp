using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class PaidConfiguration : IEntityTypeConfiguration<Paid>
{
    public void Configure(EntityTypeBuilder<Paid> builder)
    {
        builder.HasOne(p => p.User)
            .WithOne(u => u.Paid)
            .HasForeignKey<Paid>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}