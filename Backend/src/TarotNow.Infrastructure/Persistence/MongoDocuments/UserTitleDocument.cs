

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class UserTitleDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("title_code")]
    public string TitleCode { get; set; } = string.Empty;

    [BsonElement("granted_at")]
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
}
