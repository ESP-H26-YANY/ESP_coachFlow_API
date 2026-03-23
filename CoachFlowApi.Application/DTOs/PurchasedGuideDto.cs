using CoachFlowApi.Domain.Entities;

namespace CoachFlowApi.Application.DTOS;

public class PurchasedGuideDto
{
    public Guid GuideId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public int Price { get; set; }
    public string CoverUrl { get; set; }
    public string LinkUrl { get; set; } 
    public DateTime PurchasedAt { get; set; }

    public PurchasedGuideDto(Purchase purchase)
    {
        GuideId = purchase.GuideId;
        Title = purchase.Guide.Title;
        Description = purchase.Guide.Description;
        Category = purchase.Guide.Category;
        Price = purchase.Guide.Price;
        CoverUrl = purchase.Guide.CoverUrl;
        LinkUrl = purchase.Guide.LinkUrl;
        PurchasedAt = purchase.DatePurchase;
    }
}