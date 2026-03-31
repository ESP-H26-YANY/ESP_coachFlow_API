namespace CoachFlowApi.Application.UseCases.User.Interfaces;

public interface ITopUpWalletUseCase
{
    Task<int> Execute(Guid userId);
}