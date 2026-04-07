

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Security;

public class JwtTokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }

        public string GenerateAccessToken(User user)
    {
        
        var secretKey = _jwtOptions.SecretKey ?? throw new InvalidOperationException("Jwt:SecretKey is missing.");
        var issuer = _jwtOptions.Issuer ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
        var audience = _jwtOptions.Audience ?? throw new InvalidOperationException("Jwt:Audience is missing.");
        var expirationMinutes = ResolveAccessTokenExpiryMinutes();

        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("username", user.Username),
            new Claim("status", user.Status.ToString()),
            new Claim(ClaimTypes.Role, user.Role), 
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
        };

        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

        private int ResolveAccessTokenExpiryMinutes()
    {
        return _jwtOptions.ExpiryMinutes > 0
            ? _jwtOptions.ExpiryMinutes
            : 15; 
    }

        public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64]; 
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
