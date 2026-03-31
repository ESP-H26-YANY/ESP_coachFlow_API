namespace CoachFlowApi.Domain.Interfaces.Repositories;
using CoachFlowApi.Domain.Entities;

public interface IPurchaseRepository
{
    Task BuyGuideAsync(Guid userId, Guid guideId);
    Task<List<Purchase>> GetPurchasesByCoachIdAsync(Guid coachId);
    
}