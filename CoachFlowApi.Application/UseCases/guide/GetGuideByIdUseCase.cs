using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.Guide.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Guide;

public class GetGuideByIdUseCase : IGetGuideByIdUseCase
{
    private readonly IGuideRepository _guideRepository;

    public GetGuideByIdUseCase(IGuideRepository guideRepository)
    {
        _guideRepository = guideRepository;
    }

    public async Task<GuideDto> Execute(Guid id)
    {
        var guide = await _guideRepository.FindById(id);
        
        if (guide == null)
        {
            throw new Exception("Guide introuvable.");
        }

        return new GuideDto(guide);
    }
}