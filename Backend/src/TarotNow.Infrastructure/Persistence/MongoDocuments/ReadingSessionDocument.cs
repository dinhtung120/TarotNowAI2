using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// MongoDB Document cho collection "reading_sessions" — Phiên đọc bài Tarot.
///
/// Đây là document phức tạp nhất: chứa câu hỏi, lá rút, kết quả AI,
/// follow-up chats, chi phí, và trạng thái refund.
///
/// Schema validator trong init.js kiểm tra spread_type enum và drawn_cards maxItems 10.
/// </summary>
public class ReadingSessionDocument
{
    [BsonId]
    public object Id { get; set; } = ObjectId.GenerateNewId();

    /// <summary>UUID user từ PostgreSQL.</summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>personal hoặc friend_chain.</summary>
    [BsonElement("session_type")]
    public string SessionType { get; set; } = "personal";

    /// <summary>
    /// daily_1 / spread_3 / spread_5 / spread_10.
    /// Dùng check daily 1-card limit (unique per user per UTC day).
    /// </summary>
    [BsonElement("spread_type")]
    public string SpreadType { get; set; } = string.Empty;

    /// <summary>Câu hỏi ban đầu — optional theo spec Phase 1.3.</summary>
    [BsonElement("question")]
    [BsonIgnoreIfNull]
    public string? Question { get; set; }

    /// <summary>
    /// Các lá bài đã rút — array of {card_id, position, is_reversed, card_level_at_reading}.
    /// MaxItems 10 (schema validator).
    /// </summary>
    [BsonElement("drawn_cards")]
    public List<DrawnCard> DrawnCards { get; set; } = new();

    /// <summary>
    /// Trạng thái AI: pending → streaming → completed / timeout / failed.
    /// Dùng để monitor và auto-refund nếu timeout.
    /// </summary>
    [BsonElement("ai_status")]
    public string AiStatus { get; set; } = "pending";

    /// <summary>Kết quả AI sau khi stream hoàn tất.</summary>
    [BsonElement("ai_result")]
    [BsonIgnoreIfNull]
    public AiResult? AiResult { get; set; }

    /// <summary>Thông tin locale phục vụ i18n fallback chain.</summary>
    [BsonElement("locale_info")]
    [BsonIgnoreIfNull]
    public LocaleInfo? LocaleInfo { get; set; }

    /// <summary>
    /// Danh sách follow-up questions — tối đa 5 theo spec.
    /// Mỗi followup có sequence number map với ai_requests.followup_sequence.
    /// </summary>
    [BsonElement("followups")]
    public List<FollowupEntry> Followups { get; set; } = new();

    /// <summary>Chi phí phiên — snapshot UI-friendly (source of truth: wallet_transactions).</summary>
    [BsonElement("cost")]
    [BsonIgnoreIfNull]
    public SessionCost? Cost { get; set; }

    /// <summary>Thông tin refund khi AI fail.</summary>
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

/// <summary>Một lá bài đã rút trong phiên.</summary>
public class DrawnCard
{
    [BsonElement("card_id")] public int CardId { get; set; }
    [BsonElement("position")] public int Position { get; set; }
    [BsonElement("is_reversed")] public bool IsReversed { get; set; }
    [BsonElement("card_level_at_reading")] public int CardLevelAtReading { get; set; } = 1;
}

/// <summary>Kết quả AI interpretation.</summary>
public class AiResult
{
    [BsonElement("summary")] public string Summary { get; set; } = string.Empty;
    [BsonElement("share_summary")][BsonIgnoreIfNull] public string? ShareSummary { get; set; }
    [BsonElement("suggested_followup")][BsonIgnoreIfNull] public string? SuggestedFollowup { get; set; }
}

/// <summary>Thông tin locale cho i18n — ARCH-4.4.6.</summary>
public class LocaleInfo
{
    [BsonElement("requested_locale")] public string RequestedLocale { get; set; } = "vi";
    [BsonElement("returned_locale")] public string ReturnedLocale { get; set; } = "vi";
    [BsonElement("fallback_reason")][BsonIgnoreIfNull] public string? FallbackReason { get; set; }
}

/// <summary>Follow-up entry — câu hỏi tiếp theo sau lần đọc bài chính.</summary>
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

/// <summary>Chi phí phiên — snapshot cho UI (source of truth: wallet_transactions).</summary>
public class SessionCost
{
    [BsonElement("currency")] public string Currency { get; set; } = string.Empty;
    [BsonElement("amount")] public long Amount { get; set; }
}

/// <summary>Thông tin refund khi AI stream fail/timeout.</summary>
public class RefundInfo
{
    [BsonElement("refunded")] public bool Refunded { get; set; }
    [BsonElement("wallet_tx_ref")][BsonIgnoreIfNull] public string? WalletTxRef { get; set; }
    [BsonElement("refunded_at")][BsonIgnoreIfNull] public DateTime? RefundedAt { get; set; }
}
