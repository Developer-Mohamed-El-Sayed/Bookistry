namespace Bookistry.API.Contracts.Authentications.Requests;

public record GoogleSignInRequest(
    string IdToken
);
