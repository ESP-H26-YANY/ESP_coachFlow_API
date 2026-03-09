using CoachFlowApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachFlowApi.Infrastructure.Data.Configurations;

public class SavedGuideConfiguration : IEntityTypeConfiguration<SavedGuide>
{
    public void Configure(EntityTypeBuilder<SavedGuide> builder)
    {
        builder.ToTable("SavedGuides");
        
        builder.HasKey(sg => new { sg.UserId, sg.GuideId });

        builder.HasOne(sg => sg.User)
            .WithMany(u => u.SavedGuides)
            .HasForeignKey(sg => sg.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sg => sg.Guide)
            .WithMany(g => g.SavedGuides)
            .HasForeignKey(sg => sg.GuideId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}