namespace Bookistry.API.Endpoints;
[Route("categories")]
[ApiController]
[EnableRateLimiting(RateLimit.TokenLimit)] 
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;
    [HttpPost]
    [Authorize(Roles = DefaultRoles.Author.Name)]
    public async Task<IActionResult> Create([FromBody] CategoryRequest request,CancellationToken cancellationToken)
    {
        var result = await _categoryService.CreateAsunc(request, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { result.Value.Id }, result.Value) : result.ToProblem();
    }
    [HttpGet("{id}")]
    [Authorize(Roles = DefaultRoles.Reader.Name)]
    public async Task<IActionResult> Get([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var result = await _categoryService.GetAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("{id}")]
    [Authorize(Roles = DefaultRoles.Author.Name)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
