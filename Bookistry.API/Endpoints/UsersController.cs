namespace Bookistry.API.Endpoints;
[Route("users")]
[ApiController]
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    [HttpGet("{id}/unlock")]
    public async Task<IActionResult> UnlockUser([FromRoute]string id)
    {
        var result = await _userService.UnlockAsync(id);
        return result.IsSuccess ? Accepted() : result.ToProblem();
    }
    [HttpGet]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var result = await _userService.GetAllAsync(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser([FromRoute]string id)
    {
        var result = await _userService.GetAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost] // test this endpoint
    public async Task<IActionResult> CreateUser([FromBody]CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetUser), new { id = result.Value.Id }, result.Value) : result.ToProblem();
    }
    [HttpPut("{id}")] // test this endpoint
    public async Task<IActionResult> UpdateUser([FromRoute]string id, [FromBody]UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPatch("{id}/toggle-status")] // test this endpoint
    public async Task<IActionResult> ToggleUserStatus([FromRoute]string id)
    {
        var result = await _userService.ToggleStatusAsync(id);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
