

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class ReaderRequestDocument
{
        [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

        [BsonElement("status")]
    public string Status { get; set; } = "pending";

        [BsonElement("intro_text")]
    public string IntroText { get; set; } = string.Empty;

        [BsonElement("proof_documents")]
    public List<string> ProofDocuments { get; set; } = new();

        [BsonElement("admin_note")]
    [BsonIgnoreIfNull]
    public string? AdminNote { get; set; }

        [BsonElement("reviewed_by")]
    [BsonIgnoreIfNull]
    public string? ReviewedBy { get; set; }

        [BsonElement("reviewed_at")]
    [BsonIgnoreIfNull]
    public DateTime? ReviewedAt { get; set; }

        [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}
