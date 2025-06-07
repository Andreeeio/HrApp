using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class ExellImportConfiguration : IEntityTypeConfiguration<ExellImport>
{
    public void Configure(EntityTypeBuilder<ExellImport> builder)
    {
        builder.HasOne(e => e.UploadedBy)
            .WithMany(u => u.ExellImports)
            .HasForeignKey(e => e.UploadedById)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
