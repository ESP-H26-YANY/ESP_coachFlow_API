using CoachFlowApi.Application.DTOS;

namespace CoachFlowApi.Application.UseCases.Guide.Interfaces;

public interface ICreateGuideUseCase
{
    Task<GuideDto> Execute(CreateGuideDto dto);
}

public interface IDeleteGuideUseCase
{
    Task Execute(Guid id);
}