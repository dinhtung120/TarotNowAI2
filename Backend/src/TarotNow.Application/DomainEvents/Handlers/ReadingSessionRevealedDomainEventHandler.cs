using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler hậu reveal session.
/// </summary>
public sealed class ReadingSessionRevealedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReadingSessionRevealedDomainEvent>
{
    private readonly ILogger<ReadingSessionRevealedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo handler session-revealed.
    /// </summary>
    public ReadingSessionRevealedDomainEventHandler(
        ILogger<ReadingSessionRevealedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        ReadingSessionRevealedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        // Tạm tắt background precompute để tránh đua lệnh với SSE stream phía client,
        // gây lỗi không hiển thị nội dung giải bài khi request client bị chặn rate-limit.
        _logger.LogInformation(
            "Skipping background AI precompute for revealed session {SessionId}. Client SSE stream is the source of truth.",
            domainEvent.SessionId);

        return Task.CompletedTask;
    }
}
