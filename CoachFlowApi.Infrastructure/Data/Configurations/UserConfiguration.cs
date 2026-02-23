using CoachFlowApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachFlowApi.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(200); 

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(10);
            
        builder.Property(u => u.Wallet)
            .HasDefaultValue(0);

        builder.Property(u => u.CreatedAt)
            .IsRequired();
            
        builder.HasMany(u => u.Appointments)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Purchases)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}