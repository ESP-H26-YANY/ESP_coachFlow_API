using CoachFlowApi.Application.UseCases.Guide.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Guide;

public class DeleteGuideUseCase : IDeleteGuideUseCase
{
    private readonly IGuideRepository _guideRepository;
    private readonly ILibraryRepository _libraryRepository; 

    public DeleteGuideUseCase(IGuideRepository guideRepository, ILibraryRepository libraryRepository)
    {
        _guideRepository = guideRepository;
        _libraryRepository = libraryRepository;
    }

    public async Task Execute(Guid id)
    {
        if (await _libraryRepository.IsGuideSavedByAnyone(id))
            throw new Exception("Impossible de supprimer ce guide : il a été ajouté à la bibliothèque d'un ou plusieurs users.");

        await _guideRepository.Delete(id);
    }
}