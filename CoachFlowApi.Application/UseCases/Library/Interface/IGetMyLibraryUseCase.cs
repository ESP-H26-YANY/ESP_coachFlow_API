using CoachFlowApi.Application.DTOS;

namespace CoachFlowApi.Application.UseCases.Library.Interfaces;

public interface IGetMyLibraryUseCase
{
    Task<List<SavedGuideDto>> Execute(Guid userId);
}