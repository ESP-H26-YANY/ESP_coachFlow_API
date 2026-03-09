using CoachFlowApi.Domain.Entities;

namespace CoachFlowApi.Application.DTOS;

public class GuideDto
{
    public Guid Id { get; set; }
    public Guid CoachId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string LinkUrl { get; set; }
    public string CoverUrl { get; set; }
    public int Price { get; set; }

    public GuideDto() { }

    public GuideDto(Guide guide)
    {
        Id = guide.Id;
        CoachId = guide.CoachId;
        Title = guide.Title;
        Description = guide.Description;
        Category = guide.Category;
        LinkUrl = guide.LinkUrl;
        CoverUrl = guide.CoverUrl;
        Price = guide.Price;
    }
}