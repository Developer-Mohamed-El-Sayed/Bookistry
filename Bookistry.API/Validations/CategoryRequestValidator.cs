namespace Bookistry.API.Validations;

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        RuleFor(x => x.Title)
           .NotEmpty()
           .WithMessage("Title is required")
           .MaximumLength(100)
           .WithMessage("Title must not exceed 100 characters");
    }
}
