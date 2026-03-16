namespace CoachFlowApi.Application.UseCases.Library.Interfaces;

public interface IPurchaseGuideUseCase
{
    Task Execute(Guid userId, Guid guideId);
}