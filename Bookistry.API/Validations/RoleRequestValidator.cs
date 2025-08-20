namespace Bookistry.API.Validations;

public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Role name is required.")
            .Length(3, 50)
            .WithMessage("Role name must be between 3 and 50 characters long.");
    }
}
