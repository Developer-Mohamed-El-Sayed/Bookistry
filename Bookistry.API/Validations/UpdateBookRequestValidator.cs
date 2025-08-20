namespace Bookistry.API.Validations;

public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
{
    public UpdateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .Length(3, 200)
            .WithMessage("Title must be between 3 and 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .Length(5, 2000)
            .WithMessage("Description must be between 5 and 2000 characters.");

        RuleFor(x => x.PageCount)
            .GreaterThan(0)
            .WithMessage("Page count must be greater than 0.")
            .LessThanOrEqualTo(5000)
            .WithMessage("Page count must be less than or equal to 5000.");

        RuleFor(x => x.CategoryDetails)
            .NotEmpty()
            .WithMessage("At least one category is required.")
            .Must(categories => categories.All(c => !string.IsNullOrWhiteSpace(c.Title)))
            .WithMessage("Each category must have a title.");

        RuleFor(x => x.CoverImage)
            .SetValidator(new ImageSizeValidator())
            .SetValidator(new ImageExtensionValidator());

        RuleFor(x => x.PdfFile)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new FileExtenstionValidator());
    }
}
