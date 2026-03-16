using CoachFlowApi.Application.UseCases.Library.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Library;

public class PurchaseGuideUseCase : IPurchaseGuideUseCase
{
    private readonly IPurchaseRepository _purchaseRepository;

    public PurchaseGuideUseCase(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task Execute(Guid userId, Guid guideId)
    {
        await _purchaseRepository.BuyGuideAsync(userId, guideId);
    }
}