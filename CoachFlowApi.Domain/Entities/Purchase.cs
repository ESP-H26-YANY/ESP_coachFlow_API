namespace CoachFlowApi.Domain.Entities;

public class Purchase
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid GuideId { get; set; }
    public DateTime DatePurchase { get; set; }

    public  User User { get; set; }
    public Guide Guide { get; set; }

    public Purchase(Guid userId, Guid guideId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        GuideId = guideId;
        DatePurchase = DateTime.UtcNow;
    }

    public Purchase() { }
}