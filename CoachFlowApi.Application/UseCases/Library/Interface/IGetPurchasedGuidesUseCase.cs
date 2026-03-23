using CoachFlowApi.Application.DTOS;

namespace CoachFlowApi.Application.UseCases.Library.Interfaces;

public interface IGetPurchasedGuidesUseCase
{
    Task<List<PurchasedGuideDto>> Execute(Guid userId);
}