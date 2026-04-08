using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

// Triển khai push sự kiện chat/call qua SignalR để đồng bộ trạng thái hội thoại theo thời gian thực.
public class SignalRChatPushService : IChatPushService
{
    private readonly IHubContext<ChatHub> _chatHubContext;
    private readonly IHubContext<CallHub> _callHubContext;

    /// <summary>
    /// Khởi tạo dịch vụ push sự kiện chat và call.
    /// Luồng xử lý: nhận 2 hub context tách biệt để gửi đúng loại sự kiện realtime.
    /// </summary>
    public SignalRChatPushService(IHubContext<ChatHub> chatHubContext, IHubContext<CallHub> callHubContext)
    {
        _chatHubContext = chatHubContext;
        _callHubContext = callHubContext;
    }

    /// <summary>
    /// Phát tin nhắn mới tới toàn bộ kết nối đang theo dõi hội thoại.
    /// Luồng xử lý: gửi event `message.created` vào group conversation theo id.
    /// </summary>
    public async Task BroadcastMessageAsync(string conversationId, ChatMessageDto message, CancellationToken ct = default)
    {
        // Gửi theo group conversation để chỉ participant liên quan nhận được message mới.
        await _chatHubContext.Clients.Group($"conversation:{conversationId}").SendAsync("message.created", message, ct);
    }

    /// <summary>
    /// Phát tín hiệu hội thoại đã thay đổi để client refresh trạng thái liên quan.
    /// Luồng xử lý: gửi event `conversation.updated` với payload tối giản gồm id và loại cập nhật.
    /// </summary>
    public async Task BroadcastConversationUpdatedAsync(string conversationId, string updateType, CancellationToken ct = default)
    {
        await _chatHubContext.Clients.Group($"conversation:{conversationId}").SendAsync("conversation.updated", new
        {
            // Giữ tên trường rõ nghĩa để client map trực tiếp vào model realtime.
            conversationId = conversationId,
            type = updateType
        }, ct);
    }

    /// <summary>
    /// Phát tín hiệu kết thúc cuộc gọi cho các kết nối trong cùng hội thoại.
    /// Luồng xử lý: gửi event `call.ended` kèm session và reason để client đóng UI đúng trạng thái.
    /// </summary>
    public async Task BroadcastCallEndedAsync(string conversationId, CallSessionDto session, string reason, CancellationToken ct = default)
    {
        await _callHubContext.Clients.Group($"conversation:{conversationId}").SendAsync("call.ended", new
        {
            // Payload tách session/reason để client xử lý hiển thị và hậu kiểm độc lập.
            session = session,
            reason = reason
        }, ct);
    }
}
