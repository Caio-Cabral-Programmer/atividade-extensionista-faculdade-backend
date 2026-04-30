using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class JwtTokenService(IConfiguration configuration)
{
    public (string Token, DateTime ExpiresAt) GenerateToken(User user)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"]
            ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

        var expirationMinutes = int.Parse(jwtSettings["ExpirationInMinutes"] ?? "480");
        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
