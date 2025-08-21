namespace Bookistry.API.Entities;

public sealed class Subscription : Auditable
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public string UserId { get; set; } = default!;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; }
    public bool IsActive => EndDate > DateTime.UtcNow;
    public Guid PlanId { get; set; }

    public SubscriptionPlan Plan { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
    public ICollection<Payment> Payments { get; set; } = [];
}
