namespace Bookistry.API.Contracts.Authentications.Requests;

public record RefreshTokenRequest(
    string Token,
    string RefreshToken
);
