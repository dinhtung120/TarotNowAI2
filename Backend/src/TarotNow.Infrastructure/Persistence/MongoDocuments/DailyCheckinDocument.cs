using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class DailyCheckinDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    
    [BsonElement("userId")]
    public string UserId { get; set; } = string.Empty;

    
    
    [BsonElement("businessDate")]
    public string BusinessDate { get; set; } = string.Empty;

    
    [BsonElement("goldReward")]
    public long GoldReward { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
}
