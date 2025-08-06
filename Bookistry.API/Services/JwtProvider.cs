namespace Bookistry.API.Services;

public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles)
    {
        Claim[] claims = [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(nameof(roles),JsonSerializer.Serialize(roles),JsonClaimValueTypes.JsonArray),
            new(nameof(SubscriptionType),user.IsVIP ? SubscriptionType.VIP : SubscriptionType.Free)
        ];
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationPerMinutes),
            signingCredentials: signingCredentials
        );
        return (
            token: new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            expiresIn: (int)_jwtOptions.ExpirationPerMinutes * 60
        );
    }

    public string? ValidateToken(string token)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuer = false,
                ValidIssuer = _jwtOptions.Issuer,
                ValidateAudience = false,
                ValidAudience = _jwtOptions.Audience,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            return ((JwtSecurityToken)validatedToken).Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
