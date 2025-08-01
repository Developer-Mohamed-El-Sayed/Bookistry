namespace Bookistry.API.Entities;

public sealed class Payment : Auditable
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public Guid SubscriptionId { get; set; }
    public Subscription Subscription { get; set; } = default!;

    public string OrderId { get; set; } = default!;
    public string PaymentToken { get; set; } = default!;
    public string PaymentGateway { get; set; } = string.Empty;

    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }

    public string? PaymentReferenceNumber { get; set; }
}
