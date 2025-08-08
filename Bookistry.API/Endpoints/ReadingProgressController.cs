namespace Bookistry.API.Endpoints;
[Route("books/{bookId}/readingprogress")]
[ApiController]
[Authorize]
public class ReadingProgressController(IReadingProgressService readingProgress) : ControllerBase
{
    private readonly IReadingProgressService _readingProgress = readingProgress;
    [HttpPut]
    public async Task<IActionResult> UpdateReadingProgress([FromRoute]Guid bookId, [FromBody] ReadingProgressRequest request, CancellationToken cancellationToken = default)
    { 
        var result = await _readingProgress.UpdateAsync(User.GetUserId(), bookId, request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpGet]
    public async Task<IActionResult> GetReadingProgress([FromRoute]Guid bookId, CancellationToken cancellationToken = default)
    {
        var result = await _readingProgress.GetAsync(User.GetUserId(),bookId,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
