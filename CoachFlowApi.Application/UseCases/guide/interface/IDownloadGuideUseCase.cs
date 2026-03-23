namespace CoachFlowApi.Application.UseCases.Guide.Interfaces;

public interface IDownloadGuideUseCase
{
    Task<(string FilePath, string FileName)> Execute(Guid userId, string role, Guid guideId);
}