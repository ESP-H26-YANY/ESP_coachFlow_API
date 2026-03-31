using CoachFlowApi.Application.DTOS;

namespace CoachFlowApi.Application.UseCases.Coach.Interfaces;

public interface IGetCoachDashboardStatsUseCase
{
    Task<CoachDashboardDto> Execute(Guid userId);
}