using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class ExellImportConfiguration : IEntityTypeConfiguration<ExcelImport>
{
    public void Configure(EntityTypeBuilder<ExcelImport> builder)
    {
        builder.HasOne(e => e.UploadedBy)
            .WithMany(u => u.ExellImports)
            .HasForeignKey(e => e.UploadedById)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
