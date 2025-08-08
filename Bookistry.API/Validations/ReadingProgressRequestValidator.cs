namespace Bookistry.API.Validations;

public class ReadingProgressRequestValidator : AbstractValidator<ReadingProgressRequest>
{
    public ReadingProgressRequestValidator()
    {
        RuleFor(c => c.CurrentPage)
            .NotEmpty()
            .WithMessage("current page is required.");
    }
}
