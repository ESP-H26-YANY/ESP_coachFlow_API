using CoachFlowApi.Application.UseCases.User.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;

namespace CoachFlowApi.Application.UseCases.User;

public class TopUpWalletUseCase : ITopUpWalletUseCase
{
    private readonly IUserRepository _userRepository;

    public TopUpWalletUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<int> Execute(Guid userId, int amount)
    {
        if (amount <= 0) throw new Exception("Le montant doit être supérieur à zéro.");

        var user = await _userRepository.FindById(userId);
        if (user == null) throw new Exception("Utilisateur introuvable.");

        user.Wallet += amount;
        
        await _userRepository.Update(user);

        return user.Wallet;
    }
}