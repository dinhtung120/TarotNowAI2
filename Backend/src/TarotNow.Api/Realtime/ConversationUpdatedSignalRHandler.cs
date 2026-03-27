using MediatR;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

/// <summary>
/// Handler SignalR: Lắng nghe sự kiện ConversationUpdated và phát sóng tới các bên liên quan.
/// Giúp Frontend cập nhật trạng thái (Ongoing, Completed, v.v.) ngay lập tức.
/// </summary>
public sealed class ConversationUpdatedSignalRHandler : INotificationHandler<ConversationUpdatedNotification>
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IConversationRepository _conversationRepository;

    public ConversationUpdatedSignalRHandler(
        IHubContext<ChatHub> hubContext,
        IConversationRepository conversationRepository)
    {
        _hubContext = hubContext;
        _conversationRepository = conversationRepository;
    }

    public async Task Handle(ConversationUpdatedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var conversationId = domainEvent.ConversationId;

        var conversation = await _conversationRepository.GetByIdAsync(conversationId, cancellationToken);
        if (conversation == null) return;

        // Gửi thông báo tới cả User và Reader
        var participants = new[]
        {
            $"user:{conversation.UserId}",
            $"user:{conversation.ReaderId}"
        };

        await _hubContext.Clients.Groups(participants).SendAsync("conversation.updated", new
        {
            conversationId,
            type = domainEvent.Type,
            at = domainEvent.OccurredAtUtc
        }, cancellationToken);
    }
}
