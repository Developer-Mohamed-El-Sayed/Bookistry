namespace Bookistry.API.Validations;

public class RequestFiltersValidator : AbstractValidator<RequestFilters>
{
    public RequestFiltersValidator()
    {
        RuleFor(p => p.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");
    }
}

