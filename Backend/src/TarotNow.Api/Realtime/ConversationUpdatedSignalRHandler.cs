using MediatR;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

// Bridge domain event cập nhật hội thoại sang SignalR để client nhận trạng thái gần real-time.
public sealed class ConversationUpdatedSignalRHandler : INotificationHandler<ConversationUpdatedNotification>
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IConversationRepository _conversationRepository;

    /// <summary>
    /// Khởi tạo handler phát sự kiện cập nhật hội thoại ra SignalR.
    /// Luồng xử lý: nhận hub context để push realtime và repository để truy xuất participant chính xác.
    /// </summary>
    public ConversationUpdatedSignalRHandler(
        IHubContext<ChatHub> hubContext,
        IConversationRepository conversationRepository)
    {
        _hubContext = hubContext;
        _conversationRepository = conversationRepository;
    }

    /// <summary>
    /// Xử lý notification cập nhật hội thoại và phát cho các participant liên quan.
    /// Luồng xử lý: lấy conversation, xác định group theo user/reader, gửi sự kiện `conversation.updated`.
    /// </summary>
    public async Task Handle(ConversationUpdatedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var conversationId = domainEvent.ConversationId;

        var conversation = await _conversationRepository.GetByIdAsync(conversationId, cancellationToken);
        if (conversation == null)
        {
            // Edge case: conversation đã bị xóa hoặc không tồn tại, dừng để tránh push sai đích.
            return;
        }

        // Chỉ push cho hai phía tham gia hội thoại để giới hạn phạm vi phát sự kiện.
        var participants = new[]
        {
            $"user:{conversation.UserId}",
            $"user:{conversation.ReaderId}"
        };

        // Gửi payload rút gọn đủ để client đồng bộ trạng thái conversation tại thời điểm sự kiện.
        await _hubContext.Clients.Groups(participants).SendAsync("conversation.updated", new
        {
            conversationId,
            type = domainEvent.Type,
            at = domainEvent.OccurredAtUtc
        }, cancellationToken);
    }
}
