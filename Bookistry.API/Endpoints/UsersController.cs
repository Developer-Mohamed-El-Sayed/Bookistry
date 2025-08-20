namespace Bookistry.API.Endpoints;
[Route("users")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    [HttpGet("unlock/{id}")]
    public async Task<IActionResult> UnlockUser([FromRoute]string id)
    {
        var result = await _userService.UnlockAsync(id);
        return result.IsSuccess ? Accepted() : result.ToProblem();
    }

}
