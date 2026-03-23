using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.Library.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.Library;

public class GetPurchasedGuidesUseCase : IGetPurchasedGuidesUseCase
{
    private readonly ILibraryRepository _libraryRepository;

    public GetPurchasedGuidesUseCase(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }

    public async Task<List<PurchasedGuideDto>> Execute(Guid userId)
    {
        var purchases = await _libraryRepository.GetPurchasesByUser(userId);
        return purchases.Select(p => new PurchasedGuideDto(p)).ToList();
    }
}