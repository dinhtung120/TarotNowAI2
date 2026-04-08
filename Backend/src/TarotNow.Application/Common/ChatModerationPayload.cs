namespace TarotNow.Application.Common;

// Payload gửi sang luồng moderation để kiểm duyệt nội dung chat.
public class ChatModerationPayload
{
    // Định danh tin nhắn cần kiểm duyệt.
    public string MessageId { get; set; } = string.Empty;

    // Định danh hội thoại chứa tin nhắn.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh người gửi tin nhắn.
    public string SenderId { get; set; } = string.Empty;

    // Loại tin nhắn phục vụ rule moderation.
    public string Type { get; set; } = string.Empty;

    // Nội dung text cần kiểm duyệt.
    public string Content { get; set; } = string.Empty;

    // Thời điểm tạo tin nhắn để phục vụ đối soát moderation timeline.
    public DateTime CreatedAt { get; set; }
}
