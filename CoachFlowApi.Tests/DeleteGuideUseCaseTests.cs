using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using CoachFlowApi.Application.UseCases.Guide;
using CoachFlowApi.Domain.Interfaces.Repositories;
using CoachFlowApi.Domain.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoachFlowApi.Tests;

public class DeleteGuideUseCaseTests
{
    private readonly Mock<IGuideRepository> _guideRepositoryMock;
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
    private readonly Mock<ILogger<DeleteGuideUseCase>> _loggerMock;
    private readonly DeleteGuideUseCase _useCase;

    public DeleteGuideUseCaseTests()
    {
        _guideRepositoryMock = new Mock<IGuideRepository>();
        _libraryRepositoryMock = new Mock<ILibraryRepository>();
        _loggerMock = new Mock<ILogger<DeleteGuideUseCase>>();

        _useCase = new DeleteGuideUseCase(
            _guideRepositoryMock.Object,
            _libraryRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldDeleteGuide_WhenNotSavedByAnyone()
    {
        var guideId = Guid.NewGuid();
        var guide = new Guide { Id = guideId, LinkUrl = "/test.pdf", CoverUrl = "/test.jpg" };

        _guideRepositoryMock.Setup(r => r.FindById(guideId)).ReturnsAsync(guide);
        _libraryRepositoryMock.Setup(r => r.IsGuideSavedByAnyone(guideId)).ReturnsAsync(false);

        await _useCase.Execute(guideId);

        _guideRepositoryMock.Verify(r => r.Delete(guideId), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenGuideNotFound()
    {
        var guideId = Guid.NewGuid();
        _guideRepositoryMock.Setup(r => r.FindById(guideId)).ReturnsAsync((Guide?)null);

        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(guideId));
        
        Assert.Equal("Guide introuvable.", exception.Message);
        _guideRepositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenGuideIsSaved()
    {
        var guideId = Guid.NewGuid();
        var guide = new Guide { Id = guideId, LinkUrl = "/test.pdf", CoverUrl = "/test.jpg" };

        _guideRepositoryMock.Setup(r => r.FindById(guideId)).ReturnsAsync(guide);
        _libraryRepositoryMock.Setup(r => r.IsGuideSavedByAnyone(guideId)).ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(guideId));
        
        Assert.Equal("Impossible de supprimer ce guide : il a été ajouté à la bibliothèque d'un ou plusieurs users.", exception.Message);
        _guideRepositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
    }
}