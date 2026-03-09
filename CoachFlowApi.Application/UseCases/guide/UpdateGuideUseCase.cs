using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.Guide.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Guide;

public class UpdateGuideUseCase : IUpdateGuideUseCase
{
    private readonly IGuideRepository _guideRepository;

    public UpdateGuideUseCase(IGuideRepository guideRepository)
    {
        _guideRepository = guideRepository;
    }

    public async Task<GuideDto> Execute(Guid id, UpdateGuideDto dto)
    {
        var guide = await _guideRepository.FindById(id);
        
        if (guide == null)
        {
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