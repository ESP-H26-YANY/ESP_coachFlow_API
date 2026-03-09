using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.Guide.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Guide;

public class GetGuidesByUserUseCase : IGetGuidesByUserUseCase
{
    private readonly IGuideRepository _guideRepository;

    public GetGuidesByUserUseCase(IGuideRepository guideRepository)
    {
        _guideRepository = guideRepository;
    }

    public async Task<IList<GuideDto>> Execute(Guid userId)
    {
        var guides = await _guideRepository.GetByUserId(userId);
        return guides.Select(x => new GuideDto(x)).ToList();
    }
}