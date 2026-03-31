// IA sugére ce dossier pour les objets de retour spécifiques à des cas d'utilisation, 
//propriétés qui ne sont pas persistées en base de données. 
namespace CoachFlowApi.Domain.Models;

public record CoachFinancialStats(int TotalEarnings, int Revenue30Days, int TotalSold, int Sales30Days, int UniqueCustomers);
public record CoachEngagementStats(int TotalWishlisted, Guid? MostWishlistedGuideId, string MostWishlistedGuideTitle);
public record BestSellerStat(Guid GuideId, string Title, int SalesCount);