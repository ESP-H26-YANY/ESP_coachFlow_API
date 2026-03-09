namespace CoachFlowApi.Domain.Entities;

public class SavedGuide
{
    public Guid UserId { get; set; }
    public Guid GuideId { get; set; }
    public DateTime SavedAt { get; set; }

    public virtual User User { get; set; }
    public virtual Guide Guide { get; set; }

    public SavedGuide(Guid userId, Guid guideId)
    {
        UserId = userId;
        GuideId = guideId;
        SavedAt = DateTime.UtcNow;
    }

    public SavedGuide() { }
}