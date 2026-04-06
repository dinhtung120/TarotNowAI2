/*
 * ===================================================================
 * FILE: GachaLogDocument.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.MongoDocuments
 * ===================================================================
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
public class GachaLogDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("banner_code")]
    public string BannerCode { get; set; } = string.Empty;

    [BsonElement("rarity")]
    public string Rarity { get; set; } = string.Empty;

    [BsonElement("reward_type")]
    public string RewardType { get; set; } = string.Empty;

    [BsonElement("reward_value")]
    public string RewardValue { get; set; } = string.Empty;

    [BsonElement("spent_diamond")]
    public long SpentDiamond { get; set; }

    [BsonElement("was_pity")]
    public bool WasPity { get; set; }

    [BsonElement("rng_seed")]
    public string? RngSeed { get; set; }

    [BsonElement("created_at")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }
}
