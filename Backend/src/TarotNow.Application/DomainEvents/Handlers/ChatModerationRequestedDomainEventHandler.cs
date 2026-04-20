using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler enqueue payload moderation sau khi message được lưu thành công.
/// </summary>
public sealed class ChatModerationRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ChatModerationRequestedDomainEvent>
{
    private readonly IChatModerationQueue _chatModerationQueue;

    /// <summary>
    /// Khởi tạo handler moderation requested.
    /// </summary>
    public ChatModerationRequestedDomainEventHandler(
        IChatModerationQueue chatModerationQueue,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _chatModerationQueue = chatModerationQueue;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        ChatModerationRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _chatModerationQueue.EnqueueAsync(
            new ChatModerationPayload
            {
                MessageId = domainEvent.MessageId,
                ConversationId = domainEvent.ConversationId,
                SenderId = domainEvent.SenderId,
                Type = domainEvent.MessageType,
                Content = domainEvent.Content,
                CreatedAt = domainEvent.CreatedAtUtc
            },
            cancellationToken).AsTask();
    }
}
