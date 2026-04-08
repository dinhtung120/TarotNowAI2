namespace TarotNow.Application.Interfaces;

using TarotNow.Application.Common;

// Contract đẩy sự kiện realtime của chat để đồng bộ trạng thái giữa các client.
public interface IChatPushService
{
    /// <summary>
    /// Phát tin nhắn mới tới các client trong conversation để cập nhật giao diện tức thời.
    /// Luồng xử lý: nhận message đã lưu thành công và broadcast theo conversationId.
    /// </summary>
    Task BroadcastMessageAsync(string conversationId, ChatMessageDto message, CancellationToken ct = default);

    /// <summary>
    /// Phát sự kiện cập nhật conversation khi metadata cuộc trò chuyện thay đổi.
    /// Luồng xử lý: gửi updateType theo conversationId để client refresh phần liên quan.
    /// </summary>
    Task BroadcastConversationUpdatedAsync(string conversationId, string updateType, CancellationToken ct = default);

    /// <summary>
    /// Phát sự kiện kết thúc cuộc gọi để client đóng màn hình call đúng thời điểm.
    /// Luồng xử lý: broadcast session và reason kết thúc đến toàn bộ thành viên cuộc hội thoại.
    /// </summary>
    Task BroadcastCallEndedAsync(string conversationId, CallSessionDto session, string reason, CancellationToken ct = default);
}
