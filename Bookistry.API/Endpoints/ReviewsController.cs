namespace Bookistry.API.Endpoints;
[Route("books/{bookId}/reviews")]
[ApiController]
[Authorize]
public class ReviewsController(IReviewService reviewService) : ControllerBase
{
    private readonly IReviewService _reviewService = reviewService;
    [HttpPost]
    public async Task<IActionResult> CreateReview([FromRoute] Guid bookId, [FromBody] ReviewRequest request, CancellationToken cancellationToken)
    {
        var result = await _reviewService.CreateAsync(bookId, User.GetUserId(), request, cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpGet]
    public async Task<IActionResult> GetReviews([FromRoute] Guid bookId, [FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        var result = await _reviewService.GetCurrentReview(bookId, filters, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id}")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> Get([FromRoute] Guid bookId, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _reviewService.GetAsync(bookId, id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid bookId, [FromRoute] Guid id, [FromBody] ReviewRequest request, CancellationToken cancellationToken)
    {
        var result = await _reviewService.UpdateAsync(bookId, id, request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> Delete([FromRoute] Guid bookId, Guid id, CancellationToken cancellationToken)
    {
        var result = await _reviewService.DeleteAsync(bookId, id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("restore/{id}")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> Restore([FromRoute] Guid bookId, Guid id, CancellationToken cancellationToken)
    {
        var result = await _reviewService.RestoreAsync(bookId, id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpGet("count")]
    public async Task<IActionResult> GetCount([FromRoute] Guid bookId, CancellationToken cancellationToken)
    {
        var result = await _reviewService.GetBookReviewStatsAsync(bookId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }
}
