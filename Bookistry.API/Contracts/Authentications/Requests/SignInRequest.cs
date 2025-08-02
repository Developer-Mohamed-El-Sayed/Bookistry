namespace Bookistry.API.Contracts.Authentications.Requests;

public record SignInRequest(
    string Email,
    string Password
);
