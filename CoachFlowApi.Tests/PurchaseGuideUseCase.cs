using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using CoachFlowApi.Application.UseCases.Library;
using CoachFlowApi.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace CoachFlowApi.Tests;

public class PurchaseGuideUseCaseTests
{
    private readonly Mock<IPurchaseRepository> _purchaseRepositoryMock;
    private readonly Mock<ILogger<PurchaseGuideUseCase>> _loggerMock;
    private readonly PurchaseGuideUseCase _useCase;

    public PurchaseGuideUseCaseTests()
    {
        _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
        _loggerMock = new Mock<ILogger<PurchaseGuideUseCase>>();

        _useCase = new PurchaseGuideUseCase(_purchaseRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldCallBuyGuideAsync_WhenTransactionIsSuccessful()
    {
        var userId = Guid.NewGuid();
        var guideId = Guid.NewGuid();

        _purchaseRepositoryMock
            .Setup(r => r.BuyGuideAsync(userId, guideId))
            .Returns(Task.CompletedTask);

        await _useCase.Execute(userId, guideId);

        _purchaseRepositoryMock.Verify(r => r.BuyGuideAsync(userId, guideId), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenRepositoryThrowsException()
    {
        var userId = Guid.NewGuid();
        var guideId = Guid.NewGuid();
        var expectedException = new Exception("Solde insuffisant pour cet achat.");

        // Simule un rejet de la base de données (ex: procédure stockée qui échoue)
        _purchaseRepositoryMock
            .Setup(r => r.BuyGuideAsync(userId, guideId))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(userId, guideId));
        
        Assert.Equal("Solde insuffisant pour cet achat.", exception.Message);
    }
}