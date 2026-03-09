using CoachFlowApi.Domain.Entities;
using CoachFlowApi.Domain.Interfaces.Repositories;
using CoachFlowApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace CoachFlowApi.Infrastructure.Repositories;

public class CoachRepository : ICoachRepository
{
    private readonly AppDbContext _context;

    public CoachRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Coach?> FindByUserId(Guid userId)
    {
        return await _context.Coaches.SingleOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Coach> Add(Coach coach)
    {
        EntityEntry<Coach> newCoach = await _context.Coaches.AddAsync(coach);
        await _context.SaveChangesAsync();
        return newCoach.Entity;
    }
}