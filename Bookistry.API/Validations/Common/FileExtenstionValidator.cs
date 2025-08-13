namespace Bookistry.API.Validations.Common;

public class FileExtenstionValidator : AbstractValidator<IFormFile>
{
    public FileExtenstionValidator()
    {
        RuleFor(x => x)
      .Must((request, context) =>
      {
          using var binary = new BinaryReader(request.OpenReadStream());
          var bytes = binary.ReadBytes(4);
          var fileSignature = BitConverter.ToString(bytes);
          return fileSignature.Equals(PdfSettings.AllowedSignuture, StringComparison.OrdinalIgnoreCase);
      })
      .WithMessage("Invalid PDF file format.")
      .When(x => x is not null);
    }
}
