

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class ReportDocument
{
        [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

        [BsonElement("reporter_id")]
    public string ReporterId { get; set; } = string.Empty;

        [BsonElement("target")]
    public ReportTarget Target { get; set; } = new();

        [BsonElement("conversation_ref")]
    [BsonIgnoreIfNull]
    public string? ConversationRef { get; set; }

        [BsonElement("reason")]
    public string Reason { get; set; } = string.Empty;

        [BsonElement("status")]
    public string Status { get; set; } = "pending";

        [BsonElement("result")]
    [BsonIgnoreIfNull]
    public string? Result { get; set; }

        [BsonElement("admin_note")]
    [BsonIgnoreIfNull]
    public string? AdminNote { get; set; }

        [BsonElement("resolved_by")]
    [BsonIgnoreIfNull]
    public string? ResolvedBy { get; set; }

        [BsonElement("resolved_at")]
    public DateTime? ResolvedAt { get; set; }

        [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

public class ReportTarget
{
        [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

        [BsonElement("id")]
    public string Id { get; set; } = string.Empty;
}
