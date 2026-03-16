using CoachFlowApi.Domain.Entities;

namespace CoachFlowApi.Application.DTOS;

public class SavedGuideDto
{
    public Guid GuideId { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public int price { get; set; }
    public string CoverUrl { get; set; }
    public string LinkUrl { get; set; }
    public DateTime SavedAt { get; set; }

    public SavedGuideDto(SavedGuide savedGuide)
    {
        GuideId = savedGuide.GuideId;
        Title = savedGuide.Guide.Title;
        Category = savedGuide.Guide.Category;
        price = savedGuide.Guide.Price;
        CoverUrl = savedGuide.Guide.CoverUrl;
        LinkUrl = savedGuide.Guide.LinkUrl;
        SavedAt = savedGuide.SavedAt;
    }
}