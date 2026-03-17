namespace TarotNow.Domain.Enums;

/// <summary>
/// Loại tin nhắn trong chat 1-1.
///
/// Schema MongoDB §8 → field: type
/// Mỗi loại có ý nghĩa khác nhau trong UI rendering:
/// - text: tin nhắn thường, hiện bubble chat.
/// - system: thông báo hệ thống (auto-generated), font khác, không avatar.
/// - card_share: chia sẻ lá bài Tarot, render card component.
/// - payment_*: các events tài chính, render payment UI.
/// </summary>
public static class ChatMessageType
{
    /// <summary>Tin nhắn văn bản thường.</summary>
    public const string Text = "text";

    /// <summary>Tin nhắn hệ thống (tự động).</summary>
    public const string System = "system";

    /// <summary>Chia sẻ lá bài Tarot (kèm card data).</summary>
    public const string CardShare = "card_share";

    /// <summary>Đề xuất thanh toán — reader gửi giá.</summary>
    public const string PaymentOffer = "payment_offer";

    /// <summary>User chấp nhận thanh toán.</summary>
    public const string PaymentAccept = "payment_accept";

    /// <summary>User từ chối thanh toán.</summary>
    public const string PaymentReject = "payment_reject";

    /// <summary>Hoàn tiền tự động từ hệ thống.</summary>
    public const string SystemRefund = "system_refund";

    /// <summary>Giải phóng tiền cho reader.</summary>
    public const string SystemRelease = "system_release";

    /// <summary>Thông báo tranh chấp.</summary>
    public const string SystemDispute = "system_dispute";

    /// <summary>Kiểm tra type hợp lệ.</summary>
    public static bool IsValid(string type) =>
        type is Text or System or CardShare or PaymentOffer or PaymentAccept
            or PaymentReject or SystemRefund or SystemRelease or SystemDispute;
}
