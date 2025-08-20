namespace Bookistry.API.Endpoints;
[Route("roles")]
[ApiController]
[Authorize(Roles = DefaultRoles.Admin.Name)]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet]
    public async Task<IActionResult> GetRoles (CancellationToken cancellationToken)
    {
        var result = await _roleService.GetAllRolesAsync(cancellationToken);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRole([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await _roleService.GetRoleByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _roleService.CreateRoleAsync(request, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetRole), new { id = result.Value.Id }, result.Value) : result.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromBody] RoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _roleService.UpdateRoleAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> ToggleRole([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await _roleService.ToggleRoleAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
