using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.Guide.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Guide;

public class GetAllGuidesUseCase : IGetAllGuidesUseCase
{
    private readonly IGuideRepository _guideRepository;

    public GetAllGuidesUseCase(IGuideRepository guideRepository)
    {
        _guideRepository = guideRepository;
    }

    public async Task<IList<GuideDto>> Execute()
    {
        var guides = await _guideRepository.GetAll();
        return guides.Select(x => new GuideDto(x)).ToList();
    }
}