using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.User.Interfaces;
using CoachFlowApi.Application.UseCases.Coach.Interfaces;
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
    private readonly IGetCoachDashboardStatsUseCase _dashboardStatsUseCase;

    public UserController(
        IGetUserProfileUseCase getProfileUseCase, 
        ITopUpWalletUseCase topUpUseCase, 
        IGetCoachDashboardStatsUseCase dashboardStatsUseCase)
    {
        _getProfileUseCase = getProfileUseCase;
        _topUpUseCase = topUpUseCase;
        _dashboardStatsUseCase = dashboardStatsUseCase;
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

    [HttpPost("claim-reward")]
    public async Task<IActionResult> ClaimReward()
    {
        try
        {
            var newBalance = await _topUpUseCase.Execute(GetCurrentUserId());
            return Ok(new { message = "50 points ajoutés avec succès.", newBalance = newBalance });
        }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [Authorize(Roles = "coach")]
    [HttpGet("dashboard-stats")]
    public async Task<ActionResult<CoachDashboardDto>> GetDashboardStats()
    {
        try
        {
            var stats = await _dashboardStatsUseCase.Execute(GetCurrentUserId());
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}