using CoachFlowApi.Domain.Entities;
using CoachFlowApi.Domain.Interfaces.Repositories;
using CoachFlowApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CoachFlowApi.Domain.Models;


namespace CoachFlowApi.Infrastructure.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly AppDbContext _context;

    public PurchaseRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Purchase>> GetPurchasesByCoachIdAsync(Guid coachId)
    {
        return await _context.Purchases
            .Include(p => p.Guide)
            .Where(p => p.Guide.CoachId == coachId)
            .ToListAsync();
    }

    // IA, car trop compliqué pour moi
    // Cela somme les prix de tous les guides vendus par le coach, pour les 30 derniers jours et au total, 
    // et compte le nombre de ventes et de clients uniques

    public async Task<CoachFinancialStats> GetFinancialStatsAsync(Guid coachId)
    {
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
        var baseQuery = _context.Purchases.Where(p => p.Guide.CoachId == coachId);
        var recentQuery = baseQuery.Where(p => p.DatePurchase >= thirtyDaysAgo);

        int totalEarnings = await baseQuery.SumAsync(p => (int?)p.Guide.Price) ?? 0;
        int revenue30Days = await recentQuery.SumAsync(p => (int?)p.Guide.Price) ?? 0;
        int totalSold = await baseQuery.CountAsync();
        int sales30Days = await recentQuery.CountAsync();
        int uniqueCustomers = await baseQuery.Select(p => p.UserId).Distinct().CountAsync();

        return new CoachFinancialStats(totalEarnings, revenue30Days, totalSold, sales30Days, uniqueCustomers);
    }

    // IA, car trop complexe pour moi
    // On peut faire une requete qui groupe les achats par guide, et qui compte le nombre d'achat pour chaque guide, 
    // puis on trie par ce nombre d'achat et on prend les top 
    public async Task<List<BestSellerStat>> GetBestSellersAsync(Guid coachId, int limit)
    {
        var rawResults = await _context.Purchases
            .Where(p => p.Guide.CoachId == coachId)
            .GroupBy(p => new { p.Guide.Id, p.Guide.Title })
            .Select(g => new { 
                Id = g.Key.Id, 
                Title = g.Key.Title, 
                SalesCount = g.Count() 
            })
            .OrderByDescending(x => x.SalesCount)
            .Take(limit)
            .ToListAsync();

        return rawResults
            .Select(x => new BestSellerStat(x.Id, x.Title, x.SalesCount))
            .ToList();
    }

    public async Task BuyGuideAsync(Guid userId, Guid guideId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var buyer = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var guide = await _context.Guides.FirstOrDefaultAsync(g => g.Id == guideId);

            if (buyer == null || guide == null)
                throw new Exception("Utilisateur ou guide introuvable.");

            var coach = await _context.Coaches.FirstOrDefaultAsync(c => c.Id == guide.CoachId);
            if (coach == null)
                throw new Exception("Profil Coach introuvable.");

            var seller = await _context.Users.FirstOrDefaultAsync(u => u.Id == coach.UserId);
            if (seller == null)
                throw new Exception("Compte utilisateur du coach introuvable.");

            bool alreadyPurchased = await _context.Purchases.AnyAsync(p => p.UserId == userId && p.GuideId == guideId);
            if (alreadyPurchased)
                throw new Exception("Vous avez déjà acheté ce guide.");

            if (buyer.Wallet < guide.Price)
                throw new Exception("Fonds insuffisants dans votre Wallet pour acheter ce guide.");

            buyer.Wallet -= guide.Price;
            seller.Wallet += guide.Price;

            var purchase = new Purchase(userId, guideId);
            await _context.Purchases.AddAsync(purchase);

            var favorite = await _context.SavedGuides.FirstOrDefaultAsync(sg => sg.UserId == userId && sg.GuideId == guideId);
            if (favorite != null)
            {
                _context.SavedGuides.Remove(favorite);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}