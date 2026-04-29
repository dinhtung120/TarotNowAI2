using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
public sealed class RefreshTokenDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("id")]
    public Guid Id { get; set; }

    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("session_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid SessionId { get; set; }

    [BsonElement("family_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid FamilyId { get; set; }

    [BsonElement("parent_token_id")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.String)]
    public Guid? ParentTokenId { get; set; }

    [BsonElement("replaced_by_token_id")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.String)]
    public Guid? ReplacedByTokenId { get; set; }

    [BsonElement("token_hash")]
    public string TokenHash { get; set; } = string.Empty;

    [BsonElement("expires_at_utc")]
    public DateTime ExpiresAtUtc { get; set; }

    [BsonElement("created_at_utc")]
    public DateTime CreatedAtUtc { get; set; }

    [BsonElement("created_by_ip")]
    public string CreatedByIp { get; set; } = string.Empty;

    [BsonElement("created_device_id")]
    public string CreatedDeviceId { get; set; } = string.Empty;

    [BsonElement("created_user_agent_hash")]
    public string CreatedUserAgentHash { get; set; } = string.Empty;

    [BsonElement("used_at_utc")]
    [BsonIgnoreIfNull]
    public DateTime? UsedAtUtc { get; set; }

    [BsonElement("revoked_at_utc")]
    [BsonIgnoreIfNull]
    public DateTime? RevokedAtUtc { get; set; }

    [BsonElement("revocation_reason")]
    public string RevocationReason { get; set; } = string.Empty;

    [BsonElement("last_rotate_idempotency_key")]
    public string LastRotateIdempotencyKey { get; set; } = string.Empty;
}
