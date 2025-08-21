namespace Bookistry.API.Validations;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(e => e.Email)
             .NotEmpty()
             .WithMessage("Email is required.")
             .EmailAddress()
             .WithMessage("Invalid email format.")
             .MaximumLength(100)
             .WithMessage("Email must not exceed 100 characters.");

        RuleFor(p => p.Password)
             .NotEmpty()
             .WithMessage("Password is required.")
             .Matches(RegexPatterns.Password)
             .WithMessage("Password invalid Format.");

        RuleFor(f => f.FirstName)
            .NotEmpty()
            .WithMessage("First Name required.")
            .Length(3, 100)
            .WithMessage("First Name must be between 3 and 100 characters long.");

        RuleFor(l => l.LastName)
            .NotEmpty()
            .WithMessage("Last Name Required.")
            .Length(3,100)
            .WithMessage("Last Name must be between 3 and 100 characters long.");

        RuleFor(r => r.Roles)
            .NotEmpty()
            .WithMessage("Roles are required.")
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("Roles must not contain duplicates.")
            .When(r => r.Roles != null);
    }
}
