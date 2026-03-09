using CoachFlowApi.Application.DTOS;

namespace CoachFlowApi.Application.UseCases.Guide.Interfaces;

public interface IGetGuidesByUserUseCase
{
    Task<IList<GuideDto>> Execute(Guid userId);
}