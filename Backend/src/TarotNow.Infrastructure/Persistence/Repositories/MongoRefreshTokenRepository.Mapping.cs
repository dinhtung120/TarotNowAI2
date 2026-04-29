using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed partial class MongoRefreshTokenRepository
{
    private static RefreshTokenDocument ToDocument(RefreshToken token)
    {
        return new RefreshTokenDocument
        {
            Id = token.Id,
            UserId = token.UserId,
            SessionId = token.SessionId,
            FamilyId = token.FamilyId,
            ParentTokenId = token.ParentTokenId,
            ReplacedByTokenId = token.ReplacedByTokenId,
            TokenHash = token.Token,
            ExpiresAtUtc = token.ExpiresAt,
            CreatedAtUtc = token.CreatedAt,
            CreatedByIp = token.CreatedByIp,
            CreatedDeviceId = token.CreatedDeviceId,
            CreatedUserAgentHash = token.CreatedUserAgentHash,
            UsedAtUtc = token.UsedAtUtc,
            RevokedAtUtc = token.RevokedAt,
            RevocationReason = token.RevocationReason,
            LastRotateIdempotencyKey = token.LastRotateIdempotencyKey
        };
    }

    private static RefreshToken ToEntity(RefreshTokenDocument document)
    {
        var seedToken = new RefreshToken(
            userId: document.UserId,
            token: Guid.NewGuid().ToString("N"),
            expiresAt: document.ExpiresAtUtc,
            createdByIp: document.CreatedByIp,
            sessionId: document.SessionId,
            familyId: document.FamilyId,
            parentTokenId: document.ParentTokenId,
            createdDeviceId: document.CreatedDeviceId,
            createdUserAgentHash: document.CreatedUserAgentHash);

        SetProperty(seedToken, nameof(RefreshToken.Id), document.Id);
        SetProperty(seedToken, nameof(RefreshToken.Token), document.TokenHash);
        SetProperty(seedToken, nameof(RefreshToken.CreatedAt), document.CreatedAtUtc);
        SetProperty(seedToken, nameof(RefreshToken.UsedAtUtc), document.UsedAtUtc);
        SetProperty(seedToken, nameof(RefreshToken.RevokedAt), document.RevokedAtUtc);
        SetProperty(seedToken, nameof(RefreshToken.RevocationReason), document.RevocationReason);
        SetProperty(seedToken, nameof(RefreshToken.LastRotateIdempotencyKey), document.LastRotateIdempotencyKey);
        SetProperty(seedToken, nameof(RefreshToken.ReplacedByTokenId), document.ReplacedByTokenId);

        return seedToken;
    }

    private static void SetProperty<T>(RefreshToken token, string propertyName, T value)
    {
        var propertyInfo = typeof(RefreshToken).GetProperty(propertyName, PropertyFlags);
        propertyInfo?.SetValue(token, value);
    }
}
