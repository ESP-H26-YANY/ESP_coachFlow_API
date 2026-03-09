namespace CoachFlowApi.Application.UseCases.Library.Interfaces;

public interface IRemoveFromLibraryUseCase
{
    Task Execute(Guid userId, Guid guideId);
}