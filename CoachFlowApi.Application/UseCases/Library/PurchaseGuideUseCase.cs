using CoachFlowApi.Application.UseCases.Library.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging; 

namespace CoachFlowApi.Application.UseCases.Library;

public class PurchaseGuideUseCase : IPurchaseGuideUseCase
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly ILogger<PurchaseGuideUseCase> _logger;

    public PurchaseGuideUseCase(IPurchaseRepository purchaseRepository, ILogger<PurchaseGuideUseCase> logger)
    {
        _purchaseRepository = purchaseRepository;
        _logger = logger;
    }

    public async Task Execute(Guid userId, Guid guideId)
    {
        _logger.LogInformation("Début de transaction : L'utilisateur {UserId} tente d'acheter le guide {GuideId}.", userId, guideId);

        try
        {
            await _purchaseRepository.BuyGuideAsync(userId, guideId);
            
            _logger.LogInformation("Transaction réussie : L'utilisateur {UserId} possède désormais le guide {GuideId}.", userId, guideId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Échec de la transaction : L'utilisateur {UserId} n'a pas pu acheter le guide {GuideId}. Raison : {Message}", userId, guideId, ex.Message);
            throw; 
        }
    }
}