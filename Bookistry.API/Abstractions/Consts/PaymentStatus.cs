namespace Bookistry.API.Abstractions.Consts;

public static class PaymentStatus
{
    public const string Pending = "Pending";
    public const string Completed = "Completed";
    public const string Failed = "Failed";
    public const string Refunded = "Refunded";
    public const string Cancelled = "Cancelled";
    public const string Expired = "Expired";
}
