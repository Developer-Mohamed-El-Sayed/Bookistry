namespace Bookistry.API.Contracts.Authentications.Requests;

public record SignUpRequest(
    string FullName,
    string Email,
    string Password,
    string ConfirmPassword
);
