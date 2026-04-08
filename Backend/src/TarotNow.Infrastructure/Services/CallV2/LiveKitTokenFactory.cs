using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.CallV2;

internal sealed class LiveKitTokenFactory
{
    private readonly LiveKitOptions _options;

    public LiveKitTokenFactory(IOptions<LiveKitOptions> options)
    {
        _options = options.Value;
    }

    public string CreateParticipantToken(string participantIdentity, string roomName)
    {
        var videoGrant = new Dictionary<string, object>
        {
            ["room"] = roomName,
            ["roomJoin"] = true,
            ["canPublish"] = true,
            ["canSubscribe"] = true,
            ["canPublishData"] = true,
        };

        return CreateToken(participantIdentity, videoGrant);
    }

    public string CreateRoomAdminToken(string roomName)
    {
        var videoGrant = new Dictionary<string, object>
        {
            ["room"] = roomName,
            ["roomCreate"] = true,
            ["roomAdmin"] = true,
            ["canPublishData"] = true,
        };

        return CreateToken("tarotnow-server", videoGrant);
    }

    public bool ValidateWebhookToken(string authorizationHeader, string payload)
    {
        var token = ExtractBearerToken(authorizationHeader);
        if (string.IsNullOrWhiteSpace(token)) return false;

        var principal = ValidateJwtToken(token);
        if (principal == null) return false;

        var hashClaim = principal.FindFirst("sha256")?.Value;
        if (string.IsNullOrWhiteSpace(hashClaim)) return true;
        return string.Equals(hashClaim, ComputeSha256(payload), StringComparison.Ordinal);
    }

    private string CreateToken(string subject, IDictionary<string, object> videoGrant)
    {
        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.AddMinutes(Math.Max(1, _options.TokenTtlMinutes));

        var payload = new JwtPayload
        {
            [JwtRegisteredClaimNames.Iss] = _options.ApiKey,
            [JwtRegisteredClaimNames.Sub] = subject,
            [JwtRegisteredClaimNames.Nbf] = now.ToUnixTimeSeconds(),
            [JwtRegisteredClaimNames.Exp] = expiresAt.ToUnixTimeSeconds(),
            ["video"] = videoGrant,
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.ApiSecret));
        var header = new JwtHeader(new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        var jwt = new JwtSecurityToken(header, payload);
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private ClaimsPrincipal? ValidateJwtToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _options.ApiKey,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2),
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.ApiSecret)),
        };

        var handler = new JwtSecurityTokenHandler();
        try
        {
            return handler.ValidateToken(token, validationParameters, out _);
        }
        catch
        {
            return null;
        }
    }

    private static string ExtractBearerToken(string authorizationHeader)
    {
        return authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? authorizationHeader["Bearer ".Length..].Trim()
            : string.Empty;
    }

    private static string ComputeSha256(string payload)
    {
        var data = Encoding.UTF8.GetBytes(payload);
        var hash = SHA256.HashData(data);
        return Convert.ToBase64String(hash);
    }
}
