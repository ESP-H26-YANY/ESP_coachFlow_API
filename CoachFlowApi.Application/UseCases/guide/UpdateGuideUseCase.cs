using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.Guide.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace CoachFlowApi.Application.UseCases.Guide;

public class UpdateGuideUseCase : IUpdateGuideUseCase
{
    private readonly IGuideRepository _guideRepository;
    private readonly ILogger<UpdateGuideUseCase> _logger;


    public UpdateGuideUseCase(IGuideRepository guideRepository, ILogger<UpdateGuideUseCase> logger)
    {
        _guideRepository = guideRepository;
        _logger = logger;
    }

    public async Task<GuideDto> Execute(Guid id, UpdateGuideDto dto)
    {
        var guide = await _guideRepository.FindById(id);
        
        if (guide == null)
        {
            _logger.LogWarning("Échec de la mise à jour : Le guide {GuideId} est introuvable.", id);
            throw new Exception("Guide introuvable.");
        }

        guide.Title = dto.Title;
        guide.Description = dto.Description;
        guide.Category = dto.Category;
        guide.Price = dto.Price;

        await _guideRepository.Update(guide);
        
        return new GuideDto(guide);
    }
}