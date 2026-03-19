/*
 * FILE: ReadingSessionDocument.cs
 * MỤC ĐÍCH: Schema cho collection "reading_sessions" (MongoDB).
 *   Document PHỨC TẠP NHẤT: chứa toàn bộ dữ liệu 1 phiên đọc bài Tarot.
 *
 *   MỘT PHIÊN ĐỌC BÀI GỒM:
 *   → Câu hỏi ban đầu (question)
 *   → Các lá bài đã rút (drawn_cards): vị trí, xuôi/ngược, level lá lúc rút
 *   → Kết quả AI phân tích (ai_result): summary, gợi ý follow-up
 *   → Câu hỏi tiếp (followups): tối đa 5 câu, có cả chi phí Diamond/Gold
 *   → Chi phí phiên (cost): snapshot cho UI (source of truth: wallet_transactions)
 *   → Hoàn tiền (refund): nếu AI fail/timeout → auto-refund
 *
 *   VÒNG ĐỜI AI STATUS:
 *   "pending" → "streaming" → "completed" | "timeout" | "failed"
 *   Nếu timeout/failed → auto-refund (EscrowTimerService xử lý)
 *
 *   SPREAD TYPES (loại trải bài):
 *   → "daily_1": rút 1 lá/ngày (miễn phí, giới hạn 1 lần/ngày/UTC)
 *   → "spread_3": trải 3 lá (quá khứ - hiện tại - tương lai)
 *   → "spread_5": trải 5 lá (chi tiết hơn)
 *   → "spread_10": trải 10 lá (Celtic Cross, phức tạp nhất)
 *
 *   Tham chiếu: schema.md §3 (reading_sessions)
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// 1 phiên đọc bài Tarot trong collection "reading_sessions".
/// </summary>
public class ReadingSessionDocument
{
    /// <summary>
    /// ID phiên — dùng kiểu object vì có thể là ObjectId hoặc custom ID.
    /// Mặc định tự sinh ObjectId mới.
    /// </summary>
    [BsonId]
    public object Id { get; set; } = ObjectId.GenerateNewId();

    /// <summary>UUID User thực hiện phiên đọc bài (từ PostgreSQL).</summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Loại phiên: "personal" (đọc cho bản thân) hoặc "friend_chain" (đọc cho bạn bè).
    /// Mặc định "personal".
    /// </summary>
    [BsonElement("session_type")]
    public string SessionType { get; set; } = "personal";

    /// <summary>
    /// Loại trải bài — quyết định số lá rút và chi phí:
    ///   "daily_1": 1 lá, miễn phí, giới hạn 1 lần/ngày/UTC
    ///   "spread_3": 3 lá, tốn Diamond
    ///   "spread_5": 5 lá, tốn Diamond
    ///   "spread_10": 10 lá (Celtic Cross), tốn Diamond nhiều nhất
    /// Schema validator trong MongoDB kiểm tra enum này.
    /// </summary>
    [BsonElement("spread_type")]
    public string SpreadType { get; set; } = string.Empty;

    /// <summary>
    /// Câu hỏi ban đầu của User (tùy chọn theo spec Phase 1.3).
    /// Ví dụ: "Tình cảm của tôi tháng này sẽ thế nào?"
    /// AI sẽ dùng câu hỏi này + lá bài rút được để tạo lời giải.
    /// </summary>
    [BsonElement("question")]
    [BsonIgnoreIfNull]
    public string? Question { get; set; }

    /// <summary>
    /// Danh sách lá bài đã rút — tối đa 10 lá (schema validator kiểm tra maxItems).
    /// Mỗi lá chứa: card_id, position, is_reversed, card_level_at_reading.
    /// </summary>
    [BsonElement("drawn_cards")]
    public List<DrawnCard> DrawnCards { get; set; } = new();

    /// <summary>
    /// Trạng thái AI: "pending" → "streaming" → "completed" | "timeout" | "failed".
    /// Dùng để giám sát và trigger auto-refund nếu timeout/failed.
    /// Index (ai_status, created_at) trong MongoDbContext giúp background job quét nhanh.
    /// </summary>
    [BsonElement("ai_status")]
    public string AiStatus { get; set; } = "pending";

    /// <summary>
    /// Kết quả AI sau khi stream hoàn tất.
    /// Chứa: summary (bài giải), share_summary (phiên bản ngắn để chia sẻ), suggested_followup.
    /// Null khi AI chưa xong hoặc failed.
    /// </summary>
    [BsonElement("ai_result")]
    [BsonIgnoreIfNull]
    public AiResult? AiResult { get; set; }

    /// <summary>
    /// Thông tin locale (ngôn ngữ) phục vụ i18n — ARCH-4.4.6.
    /// Ghi lại: locale User yêu cầu, locale AI thực tế trả về, lý do fallback (nếu có).
    /// </summary>
    [BsonElement("locale_info")]
    [BsonIgnoreIfNull]
    public LocaleInfo? LocaleInfo { get; set; }

    /// <summary>
    /// Danh sách câu hỏi follow-up — tối đa 5 câu theo spec.
    /// Mỗi followup có: sequence number, question, answer, cost_diamond, is_free_by_level.
    /// Sequence number ánh xạ với ai_requests.followup_sequence bên PostgreSQL.
    /// </summary>
    [BsonElement("followups")]
    public List<FollowupEntry> Followups { get; set; } = new();

    /// <summary>
    /// Chi phí phiên — snapshot cho UI (không phải source of truth).
    /// Source of truth thực sự: wallet_transactions (PostgreSQL).
    /// Lý do snapshot: UI cần hiển thị nhanh "Phiên này tốn 50 Diamond" mà không cần query PostgreSQL.
    /// </summary>
    [BsonElement("cost")]
    [BsonIgnoreIfNull]
    public SessionCost? Cost { get; set; }

    /// <summary>
    /// Thông tin hoàn tiền — chỉ có khi AI fail/timeout.
    /// Hệ thống auto-refund: trả lại Diamond/Gold cho User.
    /// </summary>
    [BsonElement("refund")]
    [BsonIgnoreIfNull]
    public RefundInfo? Refund { get; set; }

    /// <summary>Soft delete flag.</summary>
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

/// <summary>
/// 1 lá bài đã rút trong phiên. Ghi nhận trạng thái lá TẠI THỜI ĐIỂM rút (snapshot).
/// </summary>
public class DrawnCard
{
    /// <summary>ID lá bài (0-77) — tham chiếu cards_catalog._id.</summary>
    [BsonElement("card_id")] public int CardId { get; set; }
    /// <summary>Vị trí trong trải bài (1-10). VD: spread_3 → vị trí 1=quá khứ, 2=hiện tại, 3=tương lai.</summary>
    [BsonElement("position")] public int Position { get; set; }
    /// <summary>Lá có bị ngược (reversed) không — ảnh hưởng ý nghĩa của lời giải AI.</summary>
    [BsonElement("is_reversed")] public bool IsReversed { get; set; }
    /// <summary>Level lá bài TẠI THỜI ĐIỂM rút — snapshot để tính free follow-up slots chính xác.</summary>
    [BsonElement("card_level_at_reading")] public int CardLevelAtReading { get; set; } = 1;
}

/// <summary>
/// Kết quả AI interpretation — lời giải bài Tarot.
/// </summary>
public class AiResult
{
    /// <summary>Lời giải đầy đủ (có thể dài hàng trăm chữ).</summary>
    [BsonElement("summary")] public string Summary { get; set; } = string.Empty;
    /// <summary>Phiên bản ngắn gọn để chia sẻ lên mạng xã hội (1-2 câu).</summary>
    [BsonElement("share_summary")][BsonIgnoreIfNull] public string? ShareSummary { get; set; }
    /// <summary>Câu hỏi gợi ý để follow-up (AI đề xuất User có thể hỏi tiếp).</summary>
    [BsonElement("suggested_followup")][BsonIgnoreIfNull] public string? SuggestedFollowup { get; set; }
}

/// <summary>
/// Thông tin locale (ngôn ngữ) — theo ARCH-4.4.6 i18n fallback chain.
/// </summary>
public class LocaleInfo
{
    /// <summary>Locale User yêu cầu ("vi", "en", "zh").</summary>
    [BsonElement("requested_locale")] public string RequestedLocale { get; set; } = "vi";
    /// <summary>Locale AI thực tế trả về (có thể khác nếu model chưa hỗ trợ locale yêu cầu).</summary>
    [BsonElement("returned_locale")] public string ReturnedLocale { get; set; } = "vi";
    /// <summary>Lý do fallback (nếu returned ≠ requested). VD: "Model does not support zh".</summary>
    [BsonElement("fallback_reason")][BsonIgnoreIfNull] public string? FallbackReason { get; set; }
}

/// <summary>
/// Follow-up entry — câu hỏi tiếp theo sau lần đọc bài chính.
/// </summary>
public class FollowupEntry
{
    /// <summary>Số thứ tự follow-up (1-5). Map với ai_requests.followup_sequence (PostgreSQL).</summary>
    [BsonElement("sequence")] public int Sequence { get; set; }
    /// <summary>Câu hỏi User đặt.</summary>
    [BsonElement("question")] public string Question { get; set; } = string.Empty;
    /// <summary>Câu trả lời AI.</summary>
    [BsonElement("answer")] public string Answer { get; set; } = string.Empty;
    /// <summary>Chi phí Diamond cho câu hỏi này (0 nếu miễn phí nhờ card level).</summary>
    [BsonElement("cost_diamond")] public long CostDiamond { get; set; }
    /// <summary>Chi phí Gold (nếu có chương trình thanh toán bằng Gold).</summary>
    [BsonElement("cost_gold")] public long CostGold { get; set; } = 0;
    /// <summary>
    /// True nếu câu hỏi này MIỄN PHÍ nhờ card level cao.
    /// Rule: lá bài level ≥ X → được Y free follow-up slots (FollowupPricingService tính).
    /// </summary>
    [BsonElement("is_free_by_level")] public bool IsFreeByLevel { get; set; }
    /// <summary>Thời điểm câu hỏi follow-up được gửi.</summary>
    [BsonElement("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Chi phí phiên — snapshot cho UI. Source of truth: wallet_transactions (PostgreSQL).
/// </summary>
public class SessionCost
{
    /// <summary>Loại tiền: "diamond" hoặc "gold".</summary>
    [BsonElement("currency")] public string Currency { get; set; } = string.Empty;
    /// <summary>Số tiền đã trừ.</summary>
    [BsonElement("amount")] public long Amount { get; set; }
}

/// <summary>
/// Thông tin refund khi AI stream fail/timeout.
/// Hệ thống tự động hoàn tiền qua EscrowTimerService hoặc AiService.
/// </summary>
public class RefundInfo
{
    /// <summary>Đã refund hay chưa.</summary>
    [BsonElement("refunded")] public bool Refunded { get; set; }
    /// <summary>UUID wallet_transaction ghi nhận hoàn tiền (tham chiếu PostgreSQL).</summary>
    [BsonElement("wallet_tx_ref")][BsonIgnoreIfNull] public string? WalletTxRef { get; set; }
    /// <summary>Thời điểm hoàn tiền.</summary>
    [BsonElement("refunded_at")][BsonIgnoreIfNull] public DateTime? RefundedAt { get; set; }
}
