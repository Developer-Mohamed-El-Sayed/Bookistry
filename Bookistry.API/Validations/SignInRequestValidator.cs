namespace Bookistry.API.Validations;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(p => p.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Length(8,100)
            .WithMessage("Password must be between 8 and 100 characters long.")
            .Matches(RegexPatterns.Password)
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");

    }
}
