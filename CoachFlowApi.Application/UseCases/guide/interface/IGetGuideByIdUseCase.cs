using CoachFlowApi.Application.DTOS;

namespace CoachFlowApi.Application.UseCases.Guide.Interfaces;

public interface IGetGuideByIdUseCase
{
    Task<GuideDto> Execute(Guid id);
}