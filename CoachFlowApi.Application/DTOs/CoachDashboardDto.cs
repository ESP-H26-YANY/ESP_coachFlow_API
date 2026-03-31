namespace CoachFlowApi.Application.DTOS;

public class CoachDashboardDto
{
    public FinancialStatsDto Financials { get; set; } = new();
    public SalesStatsDto Sales { get; set; } = new();
    public EngagementStatsDto Engagement { get; set; } = new();
    public List<BestSellerDto> TopBestSellers { get; set; } = new();
}

public class FinancialStatsDto
{
    public int CurrentWalletBalance { get; set; }
    public int TotalLifetimeEarnings { get; set; }
    public int RevenueLast30Days { get; set; }
}

public class SalesStatsDto
{
    public int TotalActiveGuides { get; set; }
    public int TotalGuidesSold { get; set; }
    public int SalesLast30Days { get; set; }
    public int TotalUniqueCustomers { get; set; }
}

public class EngagementStatsDto
{
    public int TotalWishlisted { get; set; }
    public Guid? MostWishlistedGuideId { get; set; }
    public string MostWishlistedGuide { get; set; } = string.Empty;
    public double ConversionRatePercentage { get; set; }
}

public class BestSellerDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Sales { get; set; }
}