using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class AuthorizationConfiguration : IEntityTypeConfiguration<Authorization>
{
    public void Configure(EntityTypeBuilder<Authorization> builder)
    {
        builder.HasOne(a => a.User)
            .WithOne(u => u.Authorization)
            .HasForeignKey<Authorization>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(a => a.VerificationCode)
            .IsRequired();

        builder.Property(a => a.VerificationCodeExpiration)
            .IsRequired();
    }
}
