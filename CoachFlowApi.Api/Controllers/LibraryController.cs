using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoachFlowApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LibraryController : ControllerBase
{
    private readonly IAddToLibraryUseCase _addUseCase;
    private readonly IRemoveFromLibraryUseCase _removeUseCase;
    private readonly IGetMyLibraryUseCase _getUseCase;

    public LibraryController(
        IAddToLibraryUseCase addUseCase, 
        IRemoveFromLibraryUseCase removeUseCase, 
        IGetMyLibraryUseCase getUseCase)
    {
        _addUseCase = addUseCase;
        _removeUseCase = removeUseCase;
        _getUseCase = getUseCase;
    }

    private Guid GetCurrentUserId()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString)) throw new UnauthorizedAccessException("Utilisateur non identifié.");
        return Guid.Parse(userIdString);
    }

    [HttpPost("{guideId}")]
    public async Task<IActionResult> Add(Guid guideId)
    {
        try
        {
            await _addUseCase.Execute(GetCurrentUserId(), guideId);
            return Ok(new { message = "Guide ajouté à la bibliothèque." });
        }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpDelete("{guideId}")]
    public async Task<IActionResult> Remove(Guid guideId)
    {
        try
        {
            await _removeUseCase.Execute(GetCurrentUserId(), guideId);
            return NoContent();
        }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SavedGuideDto>>> GetMyLibrary()
    {
        try
        {
            var library = await _getUseCase.Execute(GetCurrentUserId());
            return Ok(library);
        }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }
}