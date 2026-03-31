using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.Coach.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Coach;

public class GetCoachDashboardStatsUseCase : IGetCoachDashboardStatsUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICoachRepository _coachRepository;
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly ILibraryRepository _libraryRepository;
    private readonly IGuideRepository _guideRepository;

    public GetCoachDashboardStatsUseCase(
        IUserRepository userRepository,
        ICoachRepository coachRepository,
        IPurchaseRepository purchaseRepository,
        ILibraryRepository libraryRepository,
        IGuideRepository guideRepository)
    {
        _userRepository = userRepository;
        _coachRepository = coachRepository;
        _purchaseRepository = purchaseRepository;
        _libraryRepository = libraryRepository;
        _guideRepository = guideRepository;
    }

    public async Task<CoachDashboardDto> Execute(Guid userId)
    {
        var user = await _userRepository.FindById(userId);
        var coach = await _coachRepository.FindByUserId(userId);

        if (user == null || coach == null) 
        {
            throw new Exception("Profil Coach introuvable.");
        }

        var totalActiveGuides = await _guideRepository.CountByCoachAsync(coach.Id);
        var financialStats = await _purchaseRepository.GetFinancialStatsAsync(coach.Id);
        var engagementStats = await _libraryRepository.GetEngagementStatsAsync(coach.Id);
        var bestSellers = await _purchaseRepository.GetBestSellersAsync(coach.Id, 3);

        // Calcul du taux de conversion
        // Le taux de conversion est calculé comme le nombre de guides vendus divisé par
        //  le nombre de fois où les guides ont été ajoutés à la wishlist, multiplié par 100 pour obtenir un pourcentage.
        // si 4 guides ont été vendus et qu'ils ont été ajoutés à la wishlist 20 fois, le taux de conversion serait de (4/20)*100 = 20%
        // 20% des guides ajoutés à la wishlist ont été achetés
        double conversionRate = engagementStats.TotalWishlisted > 0 
            ? Math.Round((double)financialStats.TotalSold / engagementStats.TotalWishlisted * 100, 2) 
            : 0;

        return new CoachDashboardDto
        {
            Financials = new FinancialStatsDto
            {
                CurrentWalletBalance = user.Wallet,
                TotalLifetimeEarnings = financialStats.TotalEarnings,
                RevenueLast30Days = financialStats.Revenue30Days
            },
            Sales = new SalesStatsDto
            {
                TotalActiveGuides = totalActiveGuides,
                TotalGuidesSold = financialStats.TotalSold,
                SalesLast30Days = financialStats.Sales30Days,
                TotalUniqueCustomers = financialStats.UniqueCustomers
            },
            Engagement = new EngagementStatsDto
            {
                TotalWishlisted = engagementStats.TotalWishlisted,
                MostWishlistedGuideId = engagementStats.MostWishlistedGuideId,
                MostWishlistedGuide = engagementStats.MostWishlistedGuideTitle,
                ConversionRatePercentage = conversionRate
            },
            TopBestSellers = bestSellers.Select(b => new BestSellerDto 
            { 
                Id = b.GuideId, 
                Title = b.Title, 
                Sales = b.SalesCount 
            }).ToList()
        };
    }
}