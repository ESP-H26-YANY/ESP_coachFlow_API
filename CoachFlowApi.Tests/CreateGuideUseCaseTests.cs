using Moq;
using Xunit;
using FluentValidation;
using FluentValidation.Results;
using CoachFlowApi.Application.UseCases.Guide;
using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Domain.Interfaces.Repositories;
using CoachFlowApi.Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CoachFlowApi.Tests;

// J'ai compris la globalité des test mais IA ma aidé a savoir comment faire la vérification de la validation et de la non sollicitation de la base de données en cas d'échec de validation. J'ai aussi appris à simuler 
// des retours spécifiques pour les méthodes des mocks, comme retourner un objet ou null selon le scénario testé.
public class CreateGuideUseCaseTests
{
    private readonly Mock<IGuideRepository> _guideRepositoryMock;
    private readonly Mock<ICoachRepository> _coachRepositoryMock;
    private readonly Mock<IValidator<CreateGuideDto>> _validatorMock;
    private readonly CreateGuideUseCase _useCase;

    public CreateGuideUseCaseTests()
    {
        _guideRepositoryMock = new Mock<IGuideRepository>();
        _coachRepositoryMock = new Mock<ICoachRepository>();
        _validatorMock = new Mock<IValidator<CreateGuideDto>>();

        _useCase = new CreateGuideUseCase(
            _guideRepositoryMock.Object, 
            _coachRepositoryMock.Object, 
            _validatorMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldCreateGuide_WhenDataIsValidAndCoachExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var coachId = Guid.NewGuid();
        
        var dto = new CreateGuideDto 
        { 
            UserId = userId, 
            Title = "Guide Musculation", 
            Description = "Description test",
            Category = "Fitness",
            LinkUrl = "/path/to/pdf",
            CoverUrl = "/path/to/cover",
            Price = 15
        };

        var coach = new Coach(userId, "Général") { Id = coachId };
        
        // Simuler un validateur qui accepte le DTO
        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        // Simuler la présence du coach en base de données
        _coachRepositoryMock.Setup(r => r.FindByUserId(userId))
            .ReturnsAsync(coach);

        // Simuler la sauvegarde du guide
        _guideRepositoryMock.Setup(r => r.Add(It.IsAny<Guide>()))
            .ReturnsAsync((Guide g) => g); 

        // Act
        var result = await _useCase.Execute(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Title, result.Title);
        Assert.Equal(coachId, result.CoachId);

        // Vérifier que le guide a bien été envoyé pour sauvegarde
        _guideRepositoryMock.Verify(r => r.Add(It.Is<Guide>(g => g.Title == "Guide Musculation")), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenUserIsNotACoach()
    {
        // Arrange
        var dto = new CreateGuideDto { UserId = Guid.NewGuid() };
        
        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        // Simuler l'absence de profil coach, on dit ce qu'on veut avoir donc ici "NULL"
        _coachRepositoryMock.Setup(r => r.FindByUserId(dto.UserId))
            .ReturnsAsync((Coach?)null);

        // Résultat 
        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(dto));
        Assert.Equal("Aucun profil de coach n'est associé à cet utilisateur.", exception.Message);
        
        // S'assurer que le système n'a jamais tenté de sauvegarder un guide
        _guideRepositoryMock.Verify(r => r.Add(It.IsAny<Guide>()), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldThrowValidationException_WhenDataIsInvalid()
    {
        // Arrange
        var dto = new CreateGuideDto(); // DTO vide et invalide
        var validationFailures = new List<ValidationFailure> { new ValidationFailure("Title", "Titre requis") };
        
        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult(validationFailures));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _useCase.Execute(dto));
        
        // S'assurer qu'on ne sollicite pas la base de données si la validation échoue
        _coachRepositoryMock.Verify(r => r.FindByUserId(It.IsAny<Guid>()), Times.Never);
    }
}