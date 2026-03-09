using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.Library.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Library;

public class GetMyLibraryUseCase : IGetMyLibraryUseCase
{
    private readonly ILibraryRepository _libraryRepository;

    public GetMyLibraryUseCase(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }

    public async Task<List<SavedGuideDto>> Execute(Guid userId)
    {
        var guides = await _libraryRepository.GetByUser(userId);
        return guides.Select(g => new SavedGuideDto(g)).ToList();
    }
}