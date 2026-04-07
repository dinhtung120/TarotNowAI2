

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class ReadingSessionDocument
{
        [BsonId]
    public object Id { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

        [BsonElement("session_type")]
    public string SessionType { get; set; } = "personal";

        [BsonElement("spread_type")]
    public string SpreadType { get; set; } = string.Empty;

        [BsonElement("question")]
    [BsonIgnoreIfNull]
    public string? Question { get; set; }

        [BsonElement("drawn_cards")]
    public List<DrawnCard> DrawnCards { get; set; } = new();

        [BsonElement("ai_status")]
    public string AiStatus { get; set; } = "pending";

        [BsonElement("ai_result")]
    [BsonIgnoreIfNull]
    public AiResult? AiResult { get; set; }

        [BsonElement("locale_info")]
    [BsonIgnoreIfNull]
    public LocaleInfo? LocaleInfo { get; set; }

        [BsonElement("followups")]
    public List<FollowupEntry> Followups { get; set; } = new();

        [BsonElement("cost")]
    [BsonIgnoreIfNull]
    public SessionCost? Cost { get; set; }

        [BsonElement("refund")]
    [BsonIgnoreIfNull]
    public RefundInfo? Refund { get; set; }

        [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class DrawnCard
{
        [BsonElement("card_id")] public int CardId { get; set; }
        [BsonElement("position")] public int Position { get; set; }
        [BsonElement("is_reversed")] public bool IsReversed { get; set; }
        [BsonElement("card_level_at_reading")] public int CardLevelAtReading { get; set; } = 1;
}

public class AiResult
{
        [BsonElement("summary")] public string Summary { get; set; } = string.Empty;
        [BsonElement("share_summary")][BsonIgnoreIfNull] public string? ShareSummary { get; set; }
        [BsonElement("suggested_followup")][BsonIgnoreIfNull] public string? SuggestedFollowup { get; set; }
}

public class LocaleInfo
{
        [BsonElement("requested_locale")] public string RequestedLocale { get; set; } = "vi";
        [BsonElement("returned_locale")] public string ReturnedLocale { get; set; } = "vi";
        [BsonElement("fallback_reason")][BsonIgnoreIfNull] public string? FallbackReason { get; set; }
}

public class FollowupEntry
{
        [BsonElement("sequence")] public int Sequence { get; set; }
        [BsonElement("question")] public string Question { get; set; } = string.Empty;
        [BsonElement("answer")] public string Answer { get; set; } = string.Empty;
        [BsonElement("cost_diamond")] public long CostDiamond { get; set; }
        [BsonElement("cost_gold")] public long CostGold { get; set; } = 0;
        [BsonElement("is_free_by_level")] public bool IsFreeByLevel { get; set; }
        [BsonElement("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class SessionCost
{
        [BsonElement("currency")] public string Currency { get; set; } = string.Empty;
        [BsonElement("amount")] public long Amount { get; set; }
}

public class RefundInfo
{
        [BsonElement("refunded")] public bool Refunded { get; set; }
        [BsonElement("wallet_tx_ref")][BsonIgnoreIfNull] public string? WalletTxRef { get; set; }
        [BsonElement("refunded_at")][BsonIgnoreIfNull] public DateTime? RefundedAt { get; set; }
}
