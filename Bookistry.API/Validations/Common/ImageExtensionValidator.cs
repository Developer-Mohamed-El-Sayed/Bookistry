﻿namespace Bookistry.API.Validations.Common;

public class ImageExtensionValidator : AbstractValidator<IFormFile>
{
    public ImageExtensionValidator()
    {
        RuleFor(x => x)
          .Must((request, context) =>
          {
              BinaryReader binary = new(request.OpenReadStream());
              var bytes = binary.ReadBytes(2); // glabal signuture

              var fileSequenceHex = BitConverter.ToString(bytes);
              foreach (var signuture in FileSettings.ImageSettings.AllowedSignuture)
                  if (signuture.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
                      return true;
              return false;
          })
          .WithMessage("Cover image must be a valid PNG, JPEG, GIF, or BMP file.")
          .When(request => request is not null);
    }
}
