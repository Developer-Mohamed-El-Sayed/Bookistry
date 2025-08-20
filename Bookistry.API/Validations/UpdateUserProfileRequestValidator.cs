namespace Bookistry.API.Validations;

public class UpdateUserProfileRequestValidator : AbstractValidator<UpdateUserProfileRequest>
{
    public UpdateUserProfileRequestValidator()
    {
        RuleFor(f => f.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .Length(3,100)
            .WithMessage("First name must be between 3 and 100 characters long.");

        RuleFor(f => f.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .Length(3,100)
            .WithMessage("Last name must be between 3 and 100 characters long.");

    }
}
