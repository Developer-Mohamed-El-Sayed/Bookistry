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
}
