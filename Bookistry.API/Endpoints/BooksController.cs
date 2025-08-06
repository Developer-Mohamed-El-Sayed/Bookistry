namespace Bookistry.API.Endpoints;
[Route("books")]
[ApiController]
[EnableRateLimiting(RateLimit.TokenLimit)]
public class BooksController(IBookService bookService) : ControllerBase
{
    private readonly IBookService _bookService = bookService;
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters,CancellationToken cancellationToken)
    {
        var result = await _bookService.GetAllAsync(filters, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
