using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace CoachFlowApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IRegisterUserUseCase _registerUseCase;
    private readonly ILoginUseCase _loginUseCase;

    public AuthController(IRegisterUserUseCase registerUseCase, ILoginUseCase loginUseCase)
    {
        _registerUseCase = registerUseCase;
        _loginUseCase = loginUseCase;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            var user = await _registerUseCase.Execute(dto);
            return StatusCode(201, user);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthDto>> Login([FromBody] LoginDto dto)
    {
        try
        {
            var authDto = await _loginUseCase.Execute(dto);

            return Ok(authDto);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}