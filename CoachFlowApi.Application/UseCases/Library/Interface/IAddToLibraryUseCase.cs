namespace CoachFlowApi.Application.UseCases.Library.Interfaces;

public interface IAddToLibraryUseCase
{
    Task Execute(Guid userId, Guid guideId);
}