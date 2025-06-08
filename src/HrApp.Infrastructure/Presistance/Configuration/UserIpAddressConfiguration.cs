using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class UserIpAddressConfiguration : IEntityTypeConfiguration<UserIpAddress>
{
    public void Configure(EntityTypeBuilder<UserIpAddress> builder)
    {
        builder.HasOne(u => u.User)
            .WithMany(u => u.IpAddresses)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
