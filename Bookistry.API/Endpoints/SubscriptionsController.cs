namespace Bookistry.API.Endpoints;
[Route("subscription-plans/{planId}/subscriptions")]
[ApiController]
[Authorize]
public class SubscriptionsController(ISubscriptionService subscription) : ControllerBase
{
    private readonly ISubscriptionService _subscription = subscription;
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid planId, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var result = await _subscription.CreateAsync(userId, planId, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { subscriptionId = result.Value.Id }, result.Value) : result.ToProblem();
    }
    [HttpGet("{subscriptionId}")]
    public async Task<IActionResult> Get([FromRoute] Guid subscriptionId, CancellationToken cancellationToken = default)
    {
        var result = await _subscription.GetByIdAsync(subscriptionId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{subscriptionId}/plans")]
    public async Task<IActionResult> GetUsersByPlan([FromRoute] Guid subscriptionId,[FromRoute]Guid planId ,CancellationToken cancellationToken)
    {
        var result = await _subscription.GetUserSubscriptionsByPlanAsync(User.GetUserId(),planId,subscriptionId,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("~/subscriptions")]
    public async Task<IActionResult> GetAllForCurrentUser(CancellationToken cancellationToken = default)
    {
        var result = await _subscription.GetUserSubscriptionsAsync(User.GetUserId(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpDelete("{subscriptionId}")]
    public async Task<IActionResult> Cancel([FromRoute] Guid subscriptionId, Guid planId,CancellationToken cancellationToken = default)
    {
        var result = await _subscription.RevokeAsync(User.GetUserId(),subscriptionId,planId,cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPatch("{subscriptionId}/renew")]
    public async Task<IActionResult> Renew([FromRoute] Guid subscriptionId, Guid planId,CancellationToken cancellationToken = default)
    {
        var result = await _subscription.RenewAsync(User.GetUserId(),subscriptionId,planId,cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
