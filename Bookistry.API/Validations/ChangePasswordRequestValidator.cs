namespace Bookistry.API.Validations;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(p => p.CurrentPassword)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Length(8, 100)
            .WithMessage("Password must be between 8 and 100 characters long.");

        RuleFor(p => p.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.")
            .Length(8, 100)
            .WithMessage("New password must be between 8 and 100 characters long.")
            .Matches(RegexPatterns.Password)
            .WithMessage("New password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
    }
}
