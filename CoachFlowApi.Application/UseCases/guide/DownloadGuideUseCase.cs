using CoachFlowApi.Application.UseCases.Guide.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Guide;

public class DownloadGuideUseCase : IDownloadGuideUseCase
{
    private readonly IGuideRepository _guideRepository;
    private readonly ICoachRepository _coachRepository;
    private readonly ILibraryRepository _libraryRepository;

    public DownloadGuideUseCase(IGuideRepository guideRepository, ICoachRepository coachRepository, ILibraryRepository libraryRepository)
    {
        _guideRepository = guideRepository;
        _coachRepository = coachRepository;
        _libraryRepository = libraryRepository;
    }

    public async Task<(string FilePath, string FileName)> Execute(Guid userId, string role, Guid guideId)
    {
        var guide = await _guideRepository.FindById(guideId);
        if (guide == null) throw new Exception("Guide introuvable.");

        if (role == "user")
        {
            var purchases = await _libraryRepository.GetPurchasesByUser(userId);
            if (!purchases.Any(p => p.GuideId == guideId))
                throw new Exception("Accès refusé. Vous devez acheter ce guide pour le lire.");
        }
        else if (role == "coach")
        {
            var coach = await _coachRepository.FindByUserId(userId);
            if (coach == null || guide.CoachId != coach.Id)
                throw new Exception("Accès refusé. Ce guide ne vous appartient pas.");
        }

        // 2. RÉCUPÉRATION DU FICHIER PHYSIQUE
        // Ton DB contient le chemin relatif (ex: /uploads/guides/fichier.pdf), on extrait juste le nom
        string fileName = Path.GetFileName(guide.LinkUrl); 
        string filePath = Path.Combine("/var/www/coachflow_data/uploads/guides", fileName);

        if (!File.Exists(filePath)) 
            throw new Exception("Le fichier physique est introuvable sur le serveur.");

        return (filePath, $"{guide.Title}.pdf"); // On force le nom du fichier au titre du guide
    }
}