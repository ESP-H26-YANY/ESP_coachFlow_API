namespace CoachFlowApi.Domain.Entities;

public class Guide
{
    public Guid Id { get; set; }
    public Guid CoachId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public bool IsBeginner { get; set; }
    public string LinkUrl { get; set; }
    public int Price { get; set; }
    public string CoverUrl { get; set; }

    public Coach Coach { get; set; }

    public virtual ICollection<SavedGuide> SavedGuides { get; set; } = new List<SavedGuide>();

    public Guide(Guid coachId, string title, string description, string category, string linkUrl, string coverUrl, int price)
{
    Id = Guid.NewGuid();
    CoachId = coachId;
    Title = title;
    Description = description;
    Category = category;
    LinkUrl = linkUrl;
    CoverUrl = coverUrl; 
    Price = price;       
    IsBeginner = true; 
}

    public Guide() { }
}