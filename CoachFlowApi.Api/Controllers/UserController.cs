using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoachFlowApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class UserController : ControllerBase
{
    private readonly IGetUserProfileUseCase _getProfileUseCase;
    private readonly ITopUpWalletUseCase _topUpUseCase;

    public UserController(IGetUserProfileUseCase getProfileUseCase, ITopUpWalletUseCase topUpUseCase)
    {
        _getProfileUseCase = getProfileUseCase;
        _topUpUseCase = topUpUseCase;
    }

    private Guid GetCurrentUserId()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdString!);
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        try
        {
            var user = await _getProfileUseCase.Execute(GetCurrentUserId());
            return Ok(user);
        }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("points")]
    public async Task<IActionResult> TopUp([FromBody] TopUpDto dto)
    {
        try
        {
            var newBalance = await _topUpUseCase.Execute(GetCurrentUserId(), dto.Amount);
            return Ok(new { message = "Points ajoutés avec succès.", newBalance = newBalance });
        }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }
}