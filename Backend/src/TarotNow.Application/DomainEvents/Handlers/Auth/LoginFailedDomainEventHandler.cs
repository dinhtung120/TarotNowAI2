using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler side-effects khi login thất bại.
/// </summary>
public sealed class LoginFailedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<LoginFailedDomainEvent>
{
    private readonly ILogger<LoginFailedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo login failed event handler.
    /// </summary>
    public LoginFailedDomainEventHandler(
        ILogger<LoginFailedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        LoginFailedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Login failed. Reason={ReasonCode} IpHash={IpHash} IdentityHash={IdentityHash} OutboxId={OutboxId}",
            domainEvent.ReasonCode,
            domainEvent.IpHash,
            domainEvent.IdentityHash,
            outboxMessageId);
        return Task.CompletedTask;
    }
}
