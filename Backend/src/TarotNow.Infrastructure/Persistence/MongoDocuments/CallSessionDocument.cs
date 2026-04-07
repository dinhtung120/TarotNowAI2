using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class CallSessionDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("conversation_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ConversationId { get; set; } = string.Empty; 

    [BsonElement("initiator_id")]
    public string InitiatorId { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = "audio"; 

    [BsonElement("status")]
    public string Status { get; set; } = "requested"; 

    [BsonElement("end_reason")]
    public string? EndReason { get; set; } 

    [BsonElement("started_at")]
    public DateTime? StartedAt { get; set; }

    [BsonElement("ended_at")]
    public DateTime? EndedAt { get; set; }

        [BsonElement("duration_seconds")]
    public int? DurationSeconds { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
