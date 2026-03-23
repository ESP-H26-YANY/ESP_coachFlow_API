using CoachFlowApi.Domain.Entities;

namespace CoachFlowApi.Application.DTOS;

public class SavedGuideDto
{
    public Guid GuideId { get; set; }
    public Guid CoachId { get; set; } 
    public string Title { get; set; }
    public string Description { get; set; } 
    public string Category { get; set; }
    public bool IsBeginner { get; set; } 
    public int Price { get; set; }
    public string? CoverUrl { get; set; }
    public DateTime SavedAt { get; set; }

    public SavedGuideDto(SavedGuide savedGuide)
    {
        GuideId = savedGuide.GuideId;
        CoachId = savedGuide.Guide.CoachId;
        Title = savedGuide.Guide.Title;
        Description = savedGuide.Guide.Description;
        Category = savedGuide.Guide.Category;
        IsBeginner = savedGuide.Guide.IsBeginner;
        Price = savedGuide.Guide.Price;
        CoverUrl = savedGuide.Guide.CoverUrl;
        SavedAt = savedGuide.SavedAt;
    }
}