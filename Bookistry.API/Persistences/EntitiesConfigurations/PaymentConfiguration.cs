namespace Bookistry.API.Persistences.EntitiesConfigurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.StripePaymentIntentId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.StripeCustomerId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.StripeInvoiceId)
            .HasMaxLength(255);

        builder.Property(p => p.StripeChargeId)
            .HasMaxLength(255);

        builder.Property(p => p.PaymentMethodId)
            .HasMaxLength(255);

        builder.Property(p => p.ReceiptUrl)
            .HasMaxLength(500);

        builder.Property(p => p.StripePaymentMethodType)
            .HasMaxLength(50);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue(PaymentStatus.Pending);

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Currency)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("USD");

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.IsPaid)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.PaidAt)
            .IsRequired(false);

        builder.Property(p => p.StripeAmountReceived)
            .IsRequired(false);

        
        builder.HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId);

        builder.HasOne(p => p.Subscription)
            .WithMany(s => s.Payments)
            .HasForeignKey(p => p.SubscriptionId);



        builder.HasIndex(p => p.StripePaymentIntentId)
            .IsUnique();

        builder.HasIndex(p => p.UserId);

        builder.HasIndex(p => p.StripeCustomerId);

        builder.HasIndex(p => p.Status);

        builder.HasIndex(p => p.CreatedOn);

        builder.HasIndex(p => p.SubscriptionId);

    }
}