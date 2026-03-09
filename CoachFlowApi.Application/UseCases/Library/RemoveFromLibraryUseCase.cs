using CoachFlowApi.Application.UseCases.Library.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Library;

public class RemoveFromLibraryUseCase : IRemoveFromLibraryUseCase
{
    private readonly ILibraryRepository _libraryRepository;

    public RemoveFromLibraryUseCase(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }

    public async Task Execute(Guid userId, Guid guideId)
    {
        var existing = await _libraryRepository.Get(userId, guideId);
        if (existing == null) throw new Exception("Guide non trouvé dans votre bibliothèque.");

        await _libraryRepository.Remove(existing);
    }
}