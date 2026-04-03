using Moq;
using Xunit;
using CoachFlowApi.Application.UseCases.Library;
using CoachFlowApi.Domain.Interfaces.Repositories;
using CoachFlowApi.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace CoachFlowApi.Tests;

public class AddToLibraryUseCaseTests
{
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
    private readonly Mock<IGuideRepository> _guideRepositoryMock;
    private readonly AddToLibraryUseCase _useCase;

    public AddToLibraryUseCaseTests()
    {
        _libraryRepositoryMock = new Mock<ILibraryRepository>();
        _guideRepositoryMock = new Mock<IGuideRepository>();

        _useCase = new AddToLibraryUseCase(_libraryRepositoryMock.Object, _guideRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenGuideIsAlreadyInLibrary()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var guideId = Guid.NewGuid();
        var guide = new Guide { Id = guideId };
        var savedGuide = new SavedGuide(userId, guideId);

        _guideRepositoryMock.Setup(r => r.FindById(guideId)).ReturnsAsync(guide);
        
        _libraryRepositoryMock.Setup(r => r.Get(userId, guideId)).ReturnsAsync(savedGuide);

        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(userId, guideId));
        
        Assert.Equal("Ce guide est déjà dans votre bibliothèque.", exception.Message);
        
        _libraryRepositoryMock.Verify(r => r.Add(It.IsAny<SavedGuide>()), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldAddGuide_WhenValidAndNotInLibrary()
    {
        var userId = Guid.NewGuid();
        var guideId = Guid.NewGuid();
        var guide = new Guide { Id = guideId };

        _guideRepositoryMock.Setup(r => r.FindById(guideId)).ReturnsAsync(guide);
        
        // Simule que la bibliothèque ne contient pas encore ce guide
        _libraryRepositoryMock.Setup(r => r.Get(userId, guideId)).ReturnsAsync((SavedGuide?)null);

        await _useCase.Execute(userId, guideId);

        _libraryRepositoryMock.Verify(r => r.Add(It.Is<SavedGuide>(s => s.UserId == userId && s.GuideId == guideId)), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenGuideDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var guideId = Guid.NewGuid();

        // Simule un guide introuvable
        _guideRepositoryMock.Setup(r => r.FindById(guideId)).ReturnsAsync((Guide?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(userId, guideId));
        
        Assert.Equal("Guide introuvable.", exception.Message);
        
        // Sécurité : Optimisation, on ne consulte même pas la bibliothèque si le guide n'existe pas
        _libraryRepositoryMock.Verify(r => r.Get(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _libraryRepositoryMock.Verify(r => r.Add(It.IsAny<SavedGuide>()), Times.Never);
    }
}