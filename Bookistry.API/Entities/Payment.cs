namespace Bookistry.API.Entities;

public sealed class Payment : Auditable
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
    public Guid SubscriptionId { get; set; }
    public Subscription Subscription { get; set; } = default!;

    public string StripePaymentIntentId { get; set; } = default!;
    public string StripeCustomerId { get; set; } = default!;

    public string? StripeInvoiceId { get; set; }
    public string? StripeChargeId { get; set; }
    public string? PaymentMethodId { get; set; }

    public string Status { get; set; } = PaymentStatus.Pending;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }

    public string? ReceiptUrl { get; set; }
    public long? StripeAmountReceived { get; set; }
    public string? StripePaymentMethodType { get; set; }
}
