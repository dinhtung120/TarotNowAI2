using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

// Triển khai push sự kiện chat qua SignalR để đồng bộ trạng thái hội thoại theo thời gian thực.
public class SignalRChatPushService : IChatPushService
{
    private readonly IHubContext<ChatHub> _chatHubContext;

    /// <summary>
    /// Khởi tạo dịch vụ push sự kiện chat.
    /// Luồng xử lý: nhận chat hub context để gửi đúng kênh realtime theo conversation.
    /// </summary>
    public SignalRChatPushService(IHubContext<ChatHub> chatHubContext)
    {
        _chatHubContext = chatHubContext;
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

}
