namespace Bookistry.API.Validations;

public class SubscriptionPlanRequestValidator : AbstractValidator<SubscriptionPlanRequest>
{
    public SubscriptionPlanRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .Length(3,100)
            .WithMessage("Name must be between 3 and 100 characters long");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to 0");

        RuleFor(x => x.DurationInDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Duration must be 0 or more days");

        RuleFor(x => x.Description)
            .Length(3,500)
            .WithMessage("Description must be between 3 and 500 characters long")
            .When(x => x.Description is not null);

        RuleFor(x => x.Type)
            .Must(t => t.Equals(SubscriptionType.Free, StringComparison.OrdinalIgnoreCase) ||
                       t.Equals(SubscriptionType.VIP, StringComparison.OrdinalIgnoreCase))
            .WithMessage($"Type must be either {SubscriptionType.Free} or {SubscriptionType.VIP}");

    }
}
