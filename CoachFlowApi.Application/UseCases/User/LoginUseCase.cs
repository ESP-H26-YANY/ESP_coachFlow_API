using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.User.Interfaces;
using CoachFlowApi.Domain.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using UserEntity = CoachFlowApi.Domain.Entities.User;

namespace CoachFlowApi.Application.UseCases.User;

public class LoginUseCase : ILoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<LoginDto> _validator;
    private readonly IConfiguration _configuration;

    public LoginUseCase(IUserRepository userRepository, IValidator<LoginDto> validator, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _validator = validator;
        _configuration = configuration;
    }

    public async Task<AuthDto> Execute(LoginDto dto)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = await _userRepository.FindByEmail(dto.Email);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            throw new Exception("Email ou mot de passe incorrect.");
        }

        var token = GenerateJwtToken(user);

        return new AuthDto
        {
            Token = token,
            User = new UserDto(user)
        };
    }

    private string GenerateJwtToken(UserEntity user)
    {
        // partie aidé par GPT car assez differante de ce que j'ai l'habitude de faire
        var keyString = _configuration["Jwt:Key"] ;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // c'est les standard pour le tokken apparamement 
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"] ,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}