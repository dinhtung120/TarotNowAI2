using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// MongoDB document cho collection reports (schema §10).
///
/// Báo cáo vi phạm — admin xử lý qua queue.
/// </summary>
public class ReportDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("reporter_id")]
    public string ReporterId { get; set; } = string.Empty;

    /// <summary>Target: {type: "message"|"conversation"|"user", id: string}.</summary>
    [BsonElement("target")]
    public ReportTarget Target { get; set; } = new();

    /// <summary>ObjectId conversation liên quan (nếu chat-related).</summary>
    [BsonElement("conversation_ref")]
    [BsonIgnoreIfNull]
    public string? ConversationRef { get; set; }

    /// <summary>Lý do báo cáo.</summary>
    [BsonElement("reason")]
    public string Reason { get; set; } = string.Empty;

    /// <summary>pending | processing | resolved | rejected.</summary>
    [BsonElement("status")]
    public string Status { get; set; } = "pending";

    /// <summary>warn | freeze | refund | no_action.</summary>
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

/// <summary>Đối tượng bị báo cáo.</summary>
public class ReportTarget
{
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("id")]
    public string Id { get; set; } = string.Empty;
}
