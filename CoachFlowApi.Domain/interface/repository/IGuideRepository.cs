using CoachFlowApi.Domain.Entities;

namespace CoachFlowApi.Domain.Interfaces.Repositories;

public interface IGuideRepository
{
    Task<Guide?> FindById(Guid id);
    Task<List<Guide>> GetAll(); 
    Task<List<Guide>> GetByUserId(Guid userId);
    Task<Guide> Add(Guide guide);
    Task Update(Guide guide);
    Task Delete(Guid id);
}