namespace CoachFlowApi.Domain.Interfaces.Repositories;

public interface IPurchaseRepository
{
    Task BuyGuideAsync(Guid userId, Guid guideId);
}