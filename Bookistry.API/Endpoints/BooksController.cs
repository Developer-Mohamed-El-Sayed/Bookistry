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
    [HttpPost]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> Create([FromForm] CreateBookRequest request,CancellationToken cancellationToken)
    {
        var result = await _bookService.CreateAsync(User.GetUserId(),request,cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(Download),new {result.Value.Id},null) : result.ToProblem();
    }
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var result = await _bookService.GetAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("download/{id}")]
    [Authorize]
    public async Task<IActionResult> Download([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _bookService.DownloadAsync(id,User.GetUserId(),cancellationToken);
        return result.IsSuccess? File(result.Value.FileContent,result.Value.ContentType,result.Value.FileName) 
            : result.ToProblem();
    }
    [HttpPut("{id}")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateBookRequest request,CancellationToken cancellationToken)
    {
        var result = await _bookService.UpdateAsync(User.GetUserId(),id,request,cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _bookService.DeleteAsync(User.GetUserId(),id,cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPatch("restore/{id}")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> Restore([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _bookService.RestoreAsync(User.GetUserId(),id,cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
