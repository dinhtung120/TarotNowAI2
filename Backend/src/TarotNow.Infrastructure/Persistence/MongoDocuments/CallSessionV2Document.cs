using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public sealed class CallSessionV2Document
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("conversation_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ConversationId { get; set; } = string.Empty;

    [BsonElement("room_name")]
    public string RoomName { get; set; } = string.Empty;

    [BsonElement("initiator_id")]
    public string InitiatorId { get; set; } = string.Empty;

    [BsonElement("callee_id")]
    public string CalleeId { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = "audio";

    [BsonElement("status")]
    public string Status { get; set; } = "requested";

    [BsonElement("accepted_at")]
    public DateTime? AcceptedAt { get; set; }

    [BsonElement("connected_at")]
    public DateTime? ConnectedAt { get; set; }

    [BsonElement("ended_at")]
    public DateTime? EndedAt { get; set; }

    [BsonElement("end_reason")]
    public string? EndReason { get; set; }

    [BsonElement("initiator_joined_at")]
    public DateTime? InitiatorJoinedAt { get; set; }

    [BsonElement("callee_joined_at")]
    public DateTime? CalleeJoinedAt { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [BsonElement("is_log_created")]
    public bool IsLogCreated { get; set; }
}
