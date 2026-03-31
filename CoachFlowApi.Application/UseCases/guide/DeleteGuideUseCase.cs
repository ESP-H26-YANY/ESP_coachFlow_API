using CoachFlowApi.Application.UseCases.Guide.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Guide;

public class DeleteGuideUseCase : IDeleteGuideUseCase
{
    private readonly IGuideRepository _guideRepository;

    public DeleteGuideUseCase(IGuideRepository guideRepository)
    {
        _guideRepository = guideRepository;
    }

    public async Task Execute(Guid id)
    {
        var guide = await _guideRepository.FindById(id);
        
        if (guide == null) 
        {
            throw new Exception("Guide introuvable."); 
        }

        await _guideRepository.Delete(id);
    }
}