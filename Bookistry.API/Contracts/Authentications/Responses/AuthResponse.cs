namespace Bookistry.API.Contracts.Authentications.Responses;

public record AuthResponse(
    string Id,
    string Email,
    string FullName,
    string Token,
    int TokenExpirationPerMinute
// refresh token, refresh token expiration  Date time , etc. can be added later
);

