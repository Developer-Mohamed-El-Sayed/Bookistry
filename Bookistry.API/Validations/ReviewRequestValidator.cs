namespace Bookistry.API.Validations;

public class ReviewRequestValidator : AbstractValidator<ReviewRequest>
{
    public ReviewRequestValidator()
    {
        RuleFor(x => x.Comment)
                    .NotEmpty()
                    .WithMessage("Comment is required.")
                    .MaximumLength(1000)
                    .WithMessage("Comment must be at most 1000 characters.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5.");
    }
}
