namespace Bookistry.API.Validations;

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(f => f.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.")
            .Length(3, 100)
            .WithMessage("Full name must be between 3 and 100 characters.");

        RuleFor(e => e.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(p => p.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Length(8, 100)
            .WithMessage("Password must be between 8 and 100 characters.")
            .Matches(RegexPatterns.Password)
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");

        RuleFor(p => p.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirm password is required.")
            .Equal(p => p.Password)
            .WithMessage("Confirm password must match the password.");
    }
}
