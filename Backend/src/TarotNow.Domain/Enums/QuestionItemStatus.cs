namespace TarotNow.Domain.Enums;

/// <summary>
/// Trạng thái escrow cho từng câu hỏi (chat_question_items).
///
/// Lifecycle: pending → accepted → released/refunded/disputed
/// Mỗi transition PHẢI đi kèm wallet operation (freeze/release/refund).
/// </summary>
public static class QuestionItemStatus
{
    /// <summary>Đang chờ user accept offer.</summary>
    public const string Pending = "pending";

    /// <summary>User đã accept — diamond đã freeze.</summary>
    public const string Accepted = "accepted";

    /// <summary>Diamond đã release cho reader.</summary>
    public const string Released = "released";

    /// <summary>Diamond đã refund cho user.</summary>
    public const string Refunded = "refunded";

    /// <summary>Đang tranh chấp — admin xử lý.</summary>
    public const string Disputed = "disputed";

    public static bool IsValid(string s) =>
        s is Pending or Accepted or Released or Refunded or Disputed;
}
