namespace TarotNow.Infrastructure.Options;

// Options cấu hình moderation tự động cho chat.
public class ChatModerationOptions
{
    // Bật/tắt tính năng moderation tự động.
    public bool Enabled { get; set; } = true;

    // Kích thước tối đa hàng đợi moderation.
    public int MaxQueueSize { get; set; } = 1000;

    // Danh sách keyword dùng để phát hiện nội dung cần flag.
    public string[] Keywords { get; set; } = Array.Empty<string>();
}
