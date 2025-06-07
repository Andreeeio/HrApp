using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class GoogleOAuthTokenConfiguration : IEntityTypeConfiguration<GoogleOAuthToken>
{
    public void Configure(EntityTypeBuilder<GoogleOAuthToken> builder)
    {
        builder.HasOne(x => x.User)
            .WithMany(x => x.GoogleOAuthTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
