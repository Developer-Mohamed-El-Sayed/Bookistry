namespace Bookistry.API.Validations;

public class LogOutRequestValidator : AbstractValidator<LogOutRequest>
{
    public LogOutRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token is required.");

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}
