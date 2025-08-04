namespace Bookistry.API.Validations;

public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .Length(3,200)
            .WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .Length(5,2000)
            .WithMessage("Description must not exceed 2000 characters.");

        RuleFor(x => x.PageCount)
            .GreaterThan(0)
            .WithMessage("Page count must be greater than 0.");
    }
}
