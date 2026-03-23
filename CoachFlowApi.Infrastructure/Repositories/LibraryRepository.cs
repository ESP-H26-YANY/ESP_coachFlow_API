using CoachFlowApi.Domain.Entities;
using CoachFlowApi.Domain.Interfaces.Repositories;
using CoachFlowApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoachFlowApi.Infrastructure.Repositories;

public class LibraryRepository : ILibraryRepository
{
    private readonly AppDbContext _context;

    public LibraryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Add(SavedGuide savedGuide)
    {
        await _context.SavedGuides.AddAsync(savedGuide);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(SavedGuide savedGuide)
    {
        _context.SavedGuides.Remove(savedGuide);
        await _context.SaveChangesAsync();
    }

// un peu compliqué les sg=>
    public async Task<SavedGuide?> Get(Guid userId, Guid guideId)
    {
        return await _context.SavedGuides.FirstOrDefaultAsync(sg => sg.UserId == userId && sg.GuideId == guideId);
    }

    public async Task<List<SavedGuide>> GetByUser(Guid userId)
    {
        return await _context.SavedGuides
            .Include(sg => sg.Guide)
            .Where(sg => sg.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> IsGuideSavedByAnyone(Guid guideId)
    {
        return await _context.SavedGuides.AnyAsync(sg => sg.GuideId == guideId);
    }
    public async Task<List<Purchase>> GetPurchasesByUser(Guid userId)
    {
        return await _context.Purchases
            .Include(p => p.Guide)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.DatePurchase)
            .ToListAsync();
    }
}