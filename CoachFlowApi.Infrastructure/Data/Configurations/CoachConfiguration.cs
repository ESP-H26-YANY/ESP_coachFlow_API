using CoachFlowApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachFlowApi.Infrastructure.Data.Configurations;

public class CoachConfiguration : IEntityTypeConfiguration<Coach>
{
    public void Configure(EntityTypeBuilder<Coach> builder)
    {
        builder.ToTable("Coaches");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Specialization)
            .IsRequired()
            .HasMaxLength(40);

        builder.HasMany(c => c.Guides)
            .WithOne(g => g.Coach)
            .HasForeignKey(g => g.CoachId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Appointments)
            .WithOne(a => a.Coach)
            .HasForeignKey(a => a.CoachId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}