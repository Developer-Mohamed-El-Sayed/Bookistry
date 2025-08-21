namespace Bookistry.API.Endpoints;
[Route("subscription-plans")]
[ApiController]
[Authorize(Roles = DefaultRoles.Admin.Name)]
public class SubscriptionPlansController(IPlanService planService) : ControllerBase
{
    private readonly IPlanService _planService = planService;
    [HttpGet]
    public async Task<IActionResult> GetPlans(CancellationToken cancellationToken)
    {
        var result = await _planService.GetPlansAsync(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlan([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _planService.GetPlanAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("search")]
    public async Task<IActionResult> SearchPlans([FromQuery] string keyword , CancellationToken cancellationToken)
    {
        var result = await _planService.SearchPlansAsync(keyword, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id}/check")]
    public async Task<IActionResult> CheckPlan([FromRoute]Guid id,CancellationToken cancellationToken)
    {
        var result = await _planService.CheckPlanExistsAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("type")]
    public async Task<IActionResult> GetPlansByType([FromQuery] string type, CancellationToken cancellationToken)
    {
        var result = await _planService.GetPlansByTypeAsync(type, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost]
    public async Task<IActionResult> CreatePlan([FromBody] SubscriptionPlanRequest request, CancellationToken cancellationToken)
    {
        var result = await _planService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetPlan), new { id = result.Value.Id }, result.Value) : result.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlan([FromRoute] Guid id, [FromBody] SubscriptionPlanRequest request, CancellationToken cancellationToken)
    {
        var result = await _planService.UpdatePlanAsync(id, request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Revoke([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _planService.RevokePlanAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPatch("{id}/renew")]
    public async Task<IActionResult> Renew([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _planService.RenewPlanAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
