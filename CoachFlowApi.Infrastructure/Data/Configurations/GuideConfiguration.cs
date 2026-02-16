using CoachFlowApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachFlowApi.Infrastructure.Data.Configurations;

public class GuideConfiguration : IEntityTypeConfiguration<Guide>
{
    public void Configure(EntityTypeBuilder<Guide> builder)
    {
        builder.ToTable("Guides");
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(g => g.Description)
            .HasMaxLength(1000);

        builder.Property(g => g.Category)
            .HasMaxLength(20);
            
        builder.Property(g => g.IsBeginner)
            .IsRequired();

        builder.Property(g => g.LinkUrl)
            .HasMaxLength(100);

        builder.Property(g => g.Price)
            .HasDefaultValue(0);
    }
}