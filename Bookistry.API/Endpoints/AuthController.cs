namespace Bookistry.API.Endpoints;
[Route("auth")]
[ApiController]
public class AuthController(IAuthServices authServices) : ControllerBase
{
    private readonly IAuthServices _authServices = authServices;
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request,CancellationToken cancellationToken)
    {
        var result = await _authServices.SignUpAsync(request,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request, CancellationToken cancellationToken)
    {
        var result = await _authServices.SignInAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("sign-in-google")]
    public async Task<IActionResult> SignInGoogle([FromBody] GoogleSignInRequest request)
    {
        var result = await _authServices.SignInGoogleAsync(request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
