using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using CoachFlowApi.Application.UseCases.User;
using CoachFlowApi.Domain.Interfaces.Repositories;
using CoachFlowApi.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace CoachFlowApi.Tests;

public class TopUpWalletUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILogger<TopUpWalletUseCase>> _loggerMock;
    private readonly TopUpWalletUseCase _useCase;

    public TopUpWalletUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<TopUpWalletUseCase>>();
        
        _useCase = new TopUpWalletUseCase(_userRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldAdd50Points_WhenClaimIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User("test@test.com", "hash_password", "Test User", "user")
        {
            Id = userId,
            Wallet = 100,
            LastClaimDate = DateTime.UtcNow.AddDays(-2) 
        };

        _userRepositoryMock.Setup(r => r.FindById(userId)).ReturnsAsync(user);

        // Act
        var newBalance = await _useCase.Execute(userId);

        // Assert
        Assert.Equal(150, newBalance);
        _userRepositoryMock.Verify(r => r.Update(It.Is<User>(u => u.Wallet == 150)), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenClaimedToday()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User("test@test.com", "hash_password", "Test User", "user")
        {
            Id = userId,
            Wallet = 100,
            LastClaimDate = DateTime.UtcNow 
        };

        _userRepositoryMock.Setup(r => r.FindById(userId)).ReturnsAsync(user);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(userId));
        Assert.Equal("Vous avez déjà réclamé votre récompense aujourd'hui. Revenez demain !", exception.Message);
    }
}