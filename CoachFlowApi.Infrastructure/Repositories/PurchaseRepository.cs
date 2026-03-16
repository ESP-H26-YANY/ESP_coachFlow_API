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

    public async Task BuyGuideAsync(Guid userId, Guid guideId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 2. RÉCUPÉRATION DE L'ACHETEUR ET DU GUIDE
            var buyer = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var guide = await _context.Guides.FirstOrDefaultAsync(g => g.Id == guideId);
            
            if (buyer == null || guide == null)
                throw new Exception("Utilisateur ou guide introuvable.");

            // 3. RÉCUPÉRATION DU COMPTE USER DU COACH (LE VENDEUR)
            var coach = await _context.Coaches.FirstOrDefaultAsync(c => c.Id == guide.CoachId);
            if (coach == null)
                throw new Exception("Profil Coach introuvable.");

            var seller = await _context.Users.FirstOrDefaultAsync(u => u.Id == coach.UserId);
            if (seller == null)
                throw new Exception("Compte utilisateur du coach introuvable.");

            bool alreadyOwned = await _context.SavedGuides.AnyAsync(sg => sg.UserId == userId && sg.GuideId == guideId);
            if (alreadyOwned)
                throw new Exception("Vous possédez déjà ce guide dans votre bibliothèque.");

            if (buyer.Wallet < guide.Price)
                throw new Exception("Fonds insuffisants dans votre Wallet pour acheter ce guide.");

            buyer.Wallet -= guide.Price; 
            seller.Wallet += guide.Price;

            var savedGuide = new SavedGuide(userId, guideId);
            await _context.SavedGuides.AddAsync(savedGuide);

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