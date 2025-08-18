namespace Bookistry.API.Endpoints;
[Route("books/{bookId}/favorites")]
[ApiController]
[Authorize]
public class FavoritesController(IFavoriteService favoriteService) : ControllerBase
{
    private readonly IFavoriteService _favoriteService = favoriteService;
    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid bookId,CancellationToken cancellationToken)
    {
        var result = await _favoriteService.CreateAsync(bookId,User.GetUserId(), cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(IsFavorite),new {bookId},null!) : result.ToProblem();
    }
    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] Guid bookId, CancellationToken cancellationToken)
    {
        var result = await _favoriteService.DeleteAsync(bookId, User.GetUserId(), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpGet("check")]
    public async Task<IActionResult> IsFavorite([FromRoute] Guid bookId, CancellationToken cancellationToken)
    {
        var result = await _favoriteService.IsFavoriteAsync(bookId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet]
    [Authorize(Roles = DefaultRoles.Author.Name)]
    public async Task<IActionResult> GetUserFavorite([FromRoute] Guid bookId, CancellationToken cancellationToken)
    {
        var result = await _favoriteService.GetUserFavoriteAsync(User.GetUserId(), bookId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
