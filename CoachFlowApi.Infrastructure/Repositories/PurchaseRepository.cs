using CoachFlowApi.Domain.Entities;
using CoachFlowApi.Domain.Interfaces.Repositories;
using CoachFlowApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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