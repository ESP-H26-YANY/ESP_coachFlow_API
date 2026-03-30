using CoachFlowApi.Application.UseCases.Guide.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace CoachFlowApi.Application.UseCases.Guide;

public class DeleteGuideUseCase : IDeleteGuideUseCase
{
    private readonly IGuideRepository _guideRepository;
    private readonly ILibraryRepository _libraryRepository; 
    private readonly ILogger<DeleteGuideUseCase> _logger;


public async Task Execute(Guid id)
    {
        _logger.LogInformation("Demande de suppression reçue pour le guide {GuideId}.", id);

        var guide = await _guideRepository.FindById(id);
        if (guide == null) 
        {
            _logger.LogWarning("Échec de la suppression : Le guide {GuideId} est introuvable.", id);
            throw new Exception("Guide introuvable.");
        }

        if (await _libraryRepository.IsGuideSavedByAnyone(id))
        {
            _logger.LogWarning("Refus de suppression : Le guide {GuideId} est dans la bibliothèque d'au moins un utilisateur.", id);
            throw new Exception("Impossible de supprimer ce guide : il a été ajouté à la bibliothèque d'un ou plusieurs users.");
        }

        string basePath = "/var/www/coachflow_data";
        
        string pdfPath = basePath + guide.LinkUrl;
        string coverPath = basePath + guide.CoverUrl;

        if (File.Exists(pdfPath)) 
        {
            File.Delete(pdfPath);
            _logger.LogInformation("Fichier PDF supprimé : {FilePath}", pdfPath);
        }

        if (File.Exists(coverPath)) 
        {
            File.Delete(coverPath);
            _logger.LogInformation("Image de couverture supprimée : {FilePath}", coverPath);
        }

        await _guideRepository.Delete(id);
        _logger.LogInformation("Le guide {GuideId} a été supprimé définitivement de la base de données.", id);
    }
}