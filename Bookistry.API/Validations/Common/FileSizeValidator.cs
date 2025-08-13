namespace Bookistry.API.Validations.Common;

public class FileSizeValidator : AbstractValidator<IFormFile>
{
    public FileSizeValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("PDF file is required.")
            .Must((request, context) => request.Length <= FileSettings.PdfSettings.MaxSizeInBytes)
            .WithMessage($"PDF file size must not exceed {FileSettings.PdfSettings.MaxSizeInMB} MB.")
            .When(request => request is not null);
    }
}
