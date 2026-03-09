using CoachFlowApi.Domain.Entities;

namespace CoachFlowApi.Domain.Interfaces.Repositories;

public interface ILibraryRepository
{
    Task Add(SavedGuide savedGuide);
    Task Remove(SavedGuide savedGuide);
    Task<SavedGuide?> Get(Guid userId, Guid guideId);
    Task<List<SavedGuide>> GetByUser(Guid userId);
    Task<bool> IsGuideSavedByAnyone(Guid guideId);
}