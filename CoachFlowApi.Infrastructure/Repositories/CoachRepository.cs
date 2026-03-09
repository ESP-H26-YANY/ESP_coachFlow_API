using CoachFlowApi.Domain.Entities;
using CoachFlowApi.Domain.Interfaces.Repositories;
using CoachFlowApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CoachFlowApi.Infrastructure.Repositories;

public class CoachRepository : ICoachRepository
{
    private readonly AppDbContext _context;

    public CoachRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Coach> Add(Coach coach)
    {
        EntityEntry<Coach> newCoach = await _context.Coaches.AddAsync(coach);
        await _context.SaveChangesAsync();
        return newCoach.Entity;
    }
}