using CoachFlowApi.Domain.Entities;

namespace CoachFlowApi.Application.DTOS;

public class PublicGuideDto
{
    public Guid Id { get; set; }
    public Guid CoachId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public bool IsBeginner { get; set; }
    public int Price { get; set; }
    public string? CoverUrl { get; set; }

    public PublicGuideDto(Guide guide)
    {
        Id = guide.Id;
        CoachId = guide.CoachId;
        Title = guide.Title;
        Description = guide.Description;
        Category = guide.Category;
        IsBeginner = guide.IsBeginner;
        Price = guide.Price;
        CoverUrl = guide.CoverUrl;
    }
}