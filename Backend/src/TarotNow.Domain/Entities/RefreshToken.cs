

using System.Security.Cryptography;
using System.Text;
using System;

namespace TarotNow.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    
    
    public string Token { get; private set; } = string.Empty;
    
    
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public string CreatedByIp { get; private set; } = string.Empty;

        public DateTime? RevokedAt { get; private set; }

    
    public User User { get; private set; } = null!;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    protected RefreshToken() { } 

        public RefreshToken(Guid userId, string token, DateTime expiresAt, string createdByIp)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Token = HashToken(token);
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
        CreatedByIp = createdByIp;
    }

        public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }

        public bool MatchesToken(string rawToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken)) return false;

        
        if (Token.Length < 64)
            return string.Equals(Token, rawToken, StringComparison.Ordinal);

        var hashedInput = HashToken(rawToken);
        return FixedTimeEquals(Token, hashedInput);
    }

        public static string HashToken(string token)
    {
        var normalized = token.Trim();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static bool FixedTimeEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length
               && CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
