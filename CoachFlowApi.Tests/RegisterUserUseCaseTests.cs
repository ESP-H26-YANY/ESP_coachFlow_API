using Moq;
using Xunit;
using CoachFlowApi.Application.UseCases.User;
using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Domain.Interfaces.Repositories;
using CoachFlowApi.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace CoachFlowApi.Tests;

public class RegisterUserUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IValidator<RegisterUserDto>> _validatorMock;
    private readonly RegisterUserUseCase _useCase;

    public RegisterUserUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _validatorMock = new Mock<IValidator<RegisterUserDto>>();
        
        _useCase = new RegisterUserUseCase(_userRepositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var dto = new RegisterUserDto { Email = "test@test.com", Password = "password123", Name = "Test", Role = "user" };
        
        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _userRepositoryMock.Setup(r => r.FindByEmail(dto.Email))
            .ReturnsAsync(new User(dto.Email, "hashed_pass", "Existing User", "user"));

        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(dto));
        Assert.Equal("Email dejé utilisé.", exception.Message);
    }
}