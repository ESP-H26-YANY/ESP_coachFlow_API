using CoachFlowApi.Domain.Entities;
using CoachFlowApi.Domain.Interfaces.Repositories;
using CoachFlowApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CoachFlowApi.Infrastructure.Repositories;

public class GuideRepository : IGuideRepository
{
    private readonly AppDbContext _context;

    public GuideRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guide?> FindById(Guid id)
    {
        return await _context.Guides
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<List<Guide>> GetAll()
    {
        return await _context.Guides.ToListAsync();
    }

    public async Task<List<Guide>> GetByUserId(Guid userId)
    {
        return await _context.Guides
            .Where(x => x.Coach.UserId == userId)
            .ToListAsync();
    }

    public async Task<Guide> Add(Guide guide)
    {
        EntityEntry<Guide> newGuide = await _context.Guides.AddAsync(guide);
        await _context.SaveChangesAsync();
        return newGuide.Entity;
    }

    public async Task Delete(Guid id)
    {
        Guide? guide = await FindById(id);
        if (guide != null)
        {
            _context.Guides.Remove(guide);
            await _context.SaveChangesAsync();
        }
    }
}