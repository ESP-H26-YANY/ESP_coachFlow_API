using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.User.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.Results;
using BCrypt.Net;

using UserEntity = CoachFlowApi.Domain.Entities.User;

namespace CoachFlowApi.Application.UseCases.User;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<RegisterUserDto> _validator;

    public RegisterUserUseCase(IUserRepository userRepository, IValidator<RegisterUserDto> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<UserDto> Execute(RegisterUserDto dto)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var existingUser = await _userRepository.FindByEmail(dto.Email);
        if (existingUser != null)
        {
            throw new Exception("Email dejé utilisé.");
        }

        // Hashage du mot de passe ligne trouvée sur internet (GPT)
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new UserEntity(
            dto.Email, 
            passwordHash, 
            dto.Name
        );

        var savedUser = await _userRepository.Add(user);
        return new UserDto(savedUser);
    }
}