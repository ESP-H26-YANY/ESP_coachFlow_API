

[ApiController]
[Route("api/[controller]")]
public class guideController : ControllerBase
{
    private ICreateguideUseCase _createUseCase;
    private IDeleteguideUseCase _deleteUseCase;
    private IGetAllguidesUseCase _getAllUseCase;
    private IToggleguideCompleteStatusUseCase _toggleCompleteStatusUseCase;

    public guideController(ICreateguideUseCase guideCreateUseCase, IDeleteguideUseCase deleteUseCase, IGetAllguidesUseCase getAllUseCase, IToggleguideCompleteStatusUseCase toggleCompleteStatusUseCase)
    {
        _createUseCase = guideCreateUseCase;
        _deleteUseCase = deleteUseCase;
        _getAllUseCase = getAllUseCase;
        _toggleCompleteStatusUseCase = toggleCompleteStatusUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<guideDto>>> GetAll()
    {
        var jwt = Request.Cookies.First(x => x.Value == "accessToken").ToString();
        var guides = await _getAllUseCase.Execute();
        return Ok(guides);
    }

    [HttpPost]
    public async Task<ActionResult<guideDto>> Create([FromBody] CreateguideDto createguideDto)
    {
        var guide = await _createUseCase.Execute(createguideDto);

        return CreatedAtAction(
            nameof(Create),
            new { id = guide.Id },
            guide);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id)
    {
        try
        {
            await _toggleCompleteStatusUseCase.Execute(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _deleteUseCase.Execute(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

}
