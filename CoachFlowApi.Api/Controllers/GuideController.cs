using CoachFlowApi.Application.DTOS;
using CoachFlowApi.Application.UseCases.Guide.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ImageMagick;
using System.Security.Claims;

namespace CoachFlowApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuideController : ControllerBase
{
    private readonly ICreateGuideUseCase _createUseCase;
    private readonly IDeleteGuideUseCase _deleteUseCase;
    private readonly IGetAllGuidesUseCase _getAllUseCase;
    private readonly IGetGuidesByUserUseCase _getByUserUseCase;
    private readonly IGetGuideByIdUseCase _getByIdUseCase;
    private readonly IUpdateGuideUseCase _updateUseCase;
    private readonly IWebHostEnvironment _environment;

    public GuideController(
        ICreateGuideUseCase createUseCase,
        IDeleteGuideUseCase deleteUseCase,
        IGetAllGuidesUseCase getAllUseCase,
        IGetGuidesByUserUseCase getByUserUseCase,
        IGetGuideByIdUseCase getByIdUseCase,
        IUpdateGuideUseCase updateUseCase,
        IWebHostEnvironment environment)
    {
        _createUseCase = createUseCase;
        _deleteUseCase = deleteUseCase;
        _getAllUseCase = getAllUseCase;
        _getByUserUseCase = getByUserUseCase;
        _getByIdUseCase = getByIdUseCase;
        _updateUseCase = updateUseCase;
        _environment = environment;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<PublicGuideDto>>> GetAll()
    {
        try
        {
            var guides = await _getAllUseCase.Execute();
            return Ok(guides);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GuideDto>> GetById(Guid id)
    {
        try
        {
            var guide = await _getByIdUseCase.Execute(id);
            return Ok(guide);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("user/{userId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<GuideDto>>> GetByUser(Guid userId)
    {
        try
        {
            var guides = await _getByUserUseCase.Execute(userId);
            return Ok(guides);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // cette partie du code est faite par IA vu la conplexité de la gestion des fichiers,
    // elle gère l'upload du fichier PDF, la validation du fichier, 
    // et la création du guide en utilisant le use case approprié.
    // j'ai compris le code apres une longue lecture
    [HttpPost]
    [Authorize(Roles = "coach")]
    public async Task<ActionResult<GuideDto>> Create(
         IFormFile pdfFile,
         [FromForm] string title,
         [FromForm] string description,
         [FromForm] string category,
         [FromForm] int price)
    {
        try
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid currentUserId = Guid.Parse(userIdString);

            if (pdfFile == null || pdfFile.Length == 0)
                return BadRequest("Le fichier PDF est requis.");

            if (Path.GetExtension(pdfFile.FileName).ToLower() != ".pdf")
                return BadRequest("Seuls les fichiers PDF sont autorisés.");

            string uploadsFolder = "/var/www/coachflow_data/uploads/guides";

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // 1. Sauvegarde du PDF original
            string baseFileName = Guid.NewGuid().ToString();
            string pdfFileName = baseFileName + ".pdf";
            string pdfFilePath = Path.Combine(uploadsFolder, pdfFileName);

            using (var fileStream = new FileStream(pdfFilePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(fileStream);
            }

            // 2. Extraction et conversion de la page 1 en JPG
            string coverFileName = baseFileName + "_cover.jpg";
            string coverFilePath = Path.Combine(uploadsFolder, coverFileName);

            // Paramètres pour lire uniquement la première page en 150 DPI (Performance)
            var settings = new MagickReadSettings
            {
                Density = new Density(150, 150),
                FrameIndex = 0,
                FrameCount = 1
            };

            // Lecture depuis le fichier PDF sauvegardé
            using (var images = new MagickImageCollection(pdfFilePath, settings))
            {
                var firstPage = images[0];
                firstPage.Format = MagickFormat.Jpg;
                firstPage.Quality = 80;
                firstPage.Write(coverFilePath);
            }

            // 3. Préparation du DTO avec les deux URLs
            var createDto = new CreateGuideDto
            {
                UserId = currentUserId, 
                Title = title,
                Description = description,
                Category = category,
                Price = price,
                LinkUrl = $"/uploads/guides/{pdfFileName}", // Sera résolu grâce au mapping dans Program.cs
                CoverUrl = $"/uploads/guides/{coverFileName}"
            };

            var result = await _createUseCase.Execute(createDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "coach")]
    public async Task<ActionResult<GuideDto>> Update(Guid id, [FromBody] UpdateGuideDto dto)
    {
        try
        {
            var result = await _updateUseCase.Execute(id, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "coach")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _deleteUseCase.Execute(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/download")]
    [Authorize] 
    public async Task<IActionResult> DownloadPdf(Guid id, [FromServices] IDownloadGuideUseCase downloadUseCase)
    {
        try
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = Guid.Parse(userIdString!);

            var (filePath, fileName) = await downloadUseCase.Execute(currentUserId, role, id);

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // Renvoie le fichier directement dans la réponse HTTP
            return File(stream, "application/pdf", fileName); 
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}