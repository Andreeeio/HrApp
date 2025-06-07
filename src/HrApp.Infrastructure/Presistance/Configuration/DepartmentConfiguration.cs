using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasMany(d => d.Teams)
            .WithOne(t => t.Department)
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.HeadOfDepartment)
            .WithOne(u => u.Department)
            .HasForeignKey<Department>(d => d.HeadOfDepartmentId);

        builder.HasIndex(d => d.TeamTag)
            .IsUnique();

        builder.Property(d => d.TeamTag)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
