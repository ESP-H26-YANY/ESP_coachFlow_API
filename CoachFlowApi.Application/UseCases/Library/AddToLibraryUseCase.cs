using CoachFlowApi.Application.UseCases.Library.Interfaces;
using CoachFlowApi.Domain.Entities;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Library;

public class AddToLibraryUseCase : IAddToLibraryUseCase
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly IGuideRepository _guideRepository;

    public AddToLibraryUseCase(ILibraryRepository libraryRepository, IGuideRepository guideRepository)
    {
        _libraryRepository = libraryRepository;
        _guideRepository = guideRepository;
    }

    public async Task Execute(Guid userId, Guid guideId)
    {
        var guide = await _guideRepository.FindById(guideId);
        if (guide == null) throw new Exception("Guide introuvable.");

        var existing = await _libraryRepository.Get(userId, guideId);
        if (existing != null) throw new Exception("Ce guide est déjà dans votre bibliothèque.");

        await _libraryRepository.Add(new SavedGuide(userId, guideId));
    }
}