using CoachFlowApi.Application.UseCases.User.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace CoachFlowApi.Application.UseCases.User;

public class TopUpWalletUseCase : ITopUpWalletUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<TopUpWalletUseCase> _logger;

    public TopUpWalletUseCase(IUserRepository userRepository, ILogger<TopUpWalletUseCase> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<int> Execute(Guid userId)
    {
        var user = await _userRepository.FindById(userId);
        if (user == null) throw new Exception("Utilisateur introuvable.");
        _logger.LogInformation("Tentative de recharge du portefeuille pour l'utilisateur {UserId}.", userId);

        if (user.LastClaimDate.HasValue && user.LastClaimDate.Value.Date >= DateTime.UtcNow.Date)
        {
            _logger.LogWarning("Échec de la recharge : L'utilisateur {UserId} a déjà réclamé sa récompense aujourd'hui.", userId);

            throw new Exception("Vous avez déjà réclamé votre récompense aujourd'hui. Revenez demain !");
        }

        user.Wallet += 50;
        user.LastClaimDate = DateTime.UtcNow;
        
        await _userRepository.Update(user);
        _logger.LogInformation("Recharge réussie : L'utilisateur {UserId} a reçu 50 points. Nouveau solde : {Wallet}.", userId, user.Wallet);

        return user.Wallet; 
    }
}