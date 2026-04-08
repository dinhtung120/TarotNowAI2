using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

// Contract queue moderation để tách xử lý kiểm duyệt khỏi luồng gửi tin nhắn đồng bộ.
public interface IChatModerationQueue
{
    /// <summary>
    /// Đưa payload vào hàng đợi moderation để xử lý bất đồng bộ và giảm độ trễ realtime.
    /// Luồng xử lý: nhận dữ liệu moderation đã chuẩn hóa và enqueue vào backend queue.
    /// </summary>
    ValueTask EnqueueAsync(ChatModerationPayload payload, CancellationToken cancellationToken = default);
}
