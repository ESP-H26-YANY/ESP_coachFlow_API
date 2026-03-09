using CoachFlowApi.Application.DTOS;

namespace CoachFlowApi.Application.UseCases.Guide.Interfaces;

public interface IUpdateGuideUseCase
{
    Task<GuideDto> Execute(Guid id, UpdateGuideDto dto);
}