namespace ECommerce.Infrastructure.Implementations.Authentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public async Task<(string token, int ExpiresIn)> GenerateTokenAsync(ApplicationUser user, IEnumerable<string> roles)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub,user.Id),
            new(JwtRegisteredClaimNames.Email,user.Email!),
            new(JwtRegisteredClaimNames.GivenName,user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName,user.LastName),
            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new(nameof(roles),JsonSerializer.Serialize(roles),JsonClaimValueTypes.JsonArray)
        ];

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));


        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: signingCredentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), _options.ExpiryMinutes);
    }
}
