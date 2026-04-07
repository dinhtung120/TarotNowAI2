

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class UserCollectionDocument
{
        [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

        [BsonElement("card_id")]
    public int CardId { get; set; }

        [BsonElement("level")]
    public int Level { get; set; } = 1;

        [BsonElement("exp")]
    public long Exp { get; set; } = 0;

        [BsonElement("ascension_tier")]
    public int AscensionTier { get; set; } = 0;

        [BsonElement("stats")]
    public DrawStats Stats { get; set; } = new();

        [BsonElement("customization")]
    [BsonIgnoreIfNull]
    public CardCustomization? Customization { get; set; }

        [BsonElement("atk")]
    public int Atk { get; set; } = 10;

        [BsonElement("def")]
    public int Def { get; set; } = 10;

        [BsonElement("stat_history")]
    [BsonIgnoreIfNull]
    public List<StatRollRecord>? StatHistory { get; set; }

        [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

        [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("last_drawn_at")]
    public DateTime LastDrawnAt { get; set; } = DateTime.UtcNow;
}

public class DrawStats
{
        [BsonElement("times_drawn_upright")] public int TimesDrawnUpright { get; set; }
        [BsonElement("times_drawn_reversed")] public int TimesDrawnReversed { get; set; }
}

public class CardCustomization
{
        [BsonElement("signature_name")][BsonIgnoreIfNull] public string? SignatureName { get; set; }
        [BsonElement("active_skin_id")][BsonIgnoreIfNull] public string? ActiveSkinId { get; set; }
}

public class StatRollRecord
{
    [BsonElement("level")] 
    public int Level { get; set; }
    
    [BsonElement("atk_bonus")] 
    public int AtkBonus { get; set; }
    
    [BsonElement("def_bonus")] 
    public int DefBonus { get; set; }
    
    [BsonElement("rolled_at")] 
    public DateTime RolledAt { get; set; }
}
