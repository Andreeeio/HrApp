using HrApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrApp.Infrastructure.Presistance.Configuration;

public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.HasOne(c => c.JobApplication)
            .WithOne(a => a.Candidate)
            .HasForeignKey<JobApplication>(a => a.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50); 

        builder.Property(c => c.Surname)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(c => c.HomeNumber)
            .IsRequired();

        builder.Property(c => c.Street)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(c => c.City)
            .IsRequired()
            .HasMaxLength(80);
    }
}
