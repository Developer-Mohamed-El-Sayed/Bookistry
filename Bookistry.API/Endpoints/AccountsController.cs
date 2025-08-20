namespace Bookistry.API.Endpoints;
[Route("users/me")]
[ApiController]
[Authorize]
public class AccountsController(IAccountService accountService) : ControllerBase
{
    private readonly IAccountService _accountService = accountService;
    [HttpGet]
    public async Task<IActionResult> GetUserInfo()
    {
        var result = await _accountService.GetUserProfile(User.GetUserId());
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("info")]
    public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserProfileRequest request)
    {
        var result = await _accountService.UpdateProfileAsync(User.GetUserId(), request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _accountService.ChangePasswordAsync(User.GetUserId(), request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
