namespace CoachFlowApi.Domain.Interfaces.Repositories;

using CoachFlowApi.Domain.Entities;
using CoachFlowApi.Domain.Models;


public interface IPurchaseRepository
{
    Task BuyGuideAsync(Guid userId, Guid guideId);
    Task<List<Purchase>> GetPurchasesByCoachIdAsync(Guid coachId);
    Task<CoachFinancialStats> GetFinancialStatsAsync(Guid coachId);
    Task<List<BestSellerStat>> GetBestSellersAsync(Guid coachId, int limit);

}