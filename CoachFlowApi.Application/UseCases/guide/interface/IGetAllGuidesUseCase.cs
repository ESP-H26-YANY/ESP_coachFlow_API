using CoachFlowApi.Application.DTOS;

namespace CoachFlowApi.Application.UseCases.Guide.Interfaces;

public interface IGetAllGuidesUseCase
{
    Task<IList<GuideDto>> Execute();
}