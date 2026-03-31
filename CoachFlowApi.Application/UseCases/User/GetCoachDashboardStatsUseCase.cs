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

    public GetCoachDashboardStatsUseCase(
        IUserRepository userRepository,
        ICoachRepository coachRepository,
        IPurchaseRepository purchaseRepository,
        ILibraryRepository libraryRepository)
    {
        _userRepository = userRepository;
        _coachRepository = coachRepository;
        _purchaseRepository = purchaseRepository;
        _libraryRepository = libraryRepository;
    }

    public async Task<CoachDashboardDto> Execute(Guid userId)
    {
        var user = await _userRepository.FindById(userId);
        var coach = await _coachRepository.FindByUserId(userId);

        if (user == null || coach == null) throw new Exception("Profil Coach introuvable.");

        var purchases = await _purchaseRepository.GetPurchasesByCoachIdAsync(coach.Id);
        var savedGuides = await _libraryRepository.GetSavedGuidesByCoachIdAsync(coach.Id);

        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

        int totalGuidesSold = purchases.Count;
        int totalWishlisted = savedGuides.Count;

        double conversionRate = totalWishlisted > 0 
            ? Math.Round((double)totalGuidesSold / totalWishlisted * 100, 2) 
            : 0;

        var mostWishlisted = savedGuides
            .GroupBy(s => s.Guide.Title)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault();

        var bestSellers = purchases
            .GroupBy(p => p.Guide.Title)
            .Select(g => new BestSellerDto { Title = g.Key, Sales = g.Count() })
            .OrderByDescending(b => b.Sales)
            .Take(3)
            .ToList();

        return new CoachDashboardDto
        {
            Financials = new FinancialStatsDto
            {
                CurrentWalletBalance = user.Wallet,
                TotalLifetimeEarnings = purchases.Sum(p => p.Guide.Price)
            },
            Sales = new SalesStatsDto
            {
                TotalGuidesSold = totalGuidesSold,
                SalesLast30Days = purchases.Count(p => p.DatePurchase >= thirtyDaysAgo) 
            },
            Engagement = new EngagementStatsDto
            {
                TotalWishlisted = totalWishlisted,
                MostWishlistedGuide = mostWishlisted?.Key ?? "Aucun",
                ConversionRatePercentage = conversionRate
            },
            TopBestSellers = bestSellers
        };
    }
}