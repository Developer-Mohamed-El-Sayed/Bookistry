namespace Bookistry.API.Endpoints;
[Route("books/{bookId}/reviews")]
[ApiController]
[Authorize]
public class ReviewsController(IReviewService reviewService) : ControllerBase
{
    private readonly IReviewService _reviewService = reviewService;
    [HttpPost]
    public async Task<IActionResult> CreateReview([FromRoute] Guid bookId, string userId, [FromBody] ReviewRequest request, CancellationToken cancellationToken)
    {
        var result = await _reviewService.CreateAsync(bookId,User.GetUserId(), request, cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpGet]
    public async Task<IActionResult> GetReviews([FromRoute] Guid bookId, [FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        var result = await _reviewService.GetCurrentReview(bookId, filters, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
