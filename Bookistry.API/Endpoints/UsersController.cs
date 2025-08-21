namespace Bookistry.API.Endpoints;
[Route("users")]
[ApiController]
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    [HttpGet("unlock/{id}")]
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

}
