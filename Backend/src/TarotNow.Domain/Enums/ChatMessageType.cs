
namespace TarotNow.Domain.Enums;

// Tập hằng loại tin nhắn trong hội thoại chat.
public static class ChatMessageType
{
    // Tin nhắn văn bản thường.
    public const string Text = "text";

    // Tin nhắn hệ thống.
    public const string System = "system";

    // Tin nhắn chia sẻ lá bài.
    public const string CardShare = "card_share";

    // Tin nhắn ảnh.
    public const string Image = "image";

    // Tin nhắn giọng nói.
    public const string Voice = "voice";

    // Tin nhắn đề nghị thanh toán.
    public const string PaymentOffer = "payment_offer";

    // Tin nhắn chấp nhận thanh toán.
    public const string PaymentAccept = "payment_accept";

    // Tin nhắn từ chối thanh toán.
    public const string PaymentReject = "payment_reject";

    // Sự kiện hệ thống hoàn tiền.
    public const string SystemRefund = "system_refund";

    // Sự kiện hệ thống giải ngân.
    public const string SystemRelease = "system_release";

    // Sự kiện hệ thống mở tranh chấp.
    public const string SystemDispute = "system_dispute";

    // Bản ghi nhật ký cuộc gọi.
    public const string CallLog = "call_log";

    /// <summary>
    /// Kiểm tra loại tin nhắn có thuộc danh sách được hệ thống hỗ trợ hay không.
    /// Luồng xử lý: so khớp giá trị đầu vào với tập hằng đã định nghĩa và trả kết quả bool.
    /// </summary>
    public static bool IsValid(string type)
    {
        // Dùng pattern matching để kiểm tra nhanh và tránh sai lệch chuỗi loại tin nhắn.
        return type is Text or System or CardShare or Image or Voice
            or PaymentOffer or PaymentAccept or PaymentReject
            or SystemRefund or SystemRelease or SystemDispute
            or CallLog;
    }
}
