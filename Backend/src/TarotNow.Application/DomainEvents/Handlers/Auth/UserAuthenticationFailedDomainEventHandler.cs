using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed class UserAuthenticationFailedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<UserAuthenticationFailedDomainEvent>
{
    private readonly ILogger<UserAuthenticationFailedDomainEventHandler> _logger;

    public UserAuthenticationFailedDomainEventHandler(
        ILogger<UserAuthenticationFailedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _logger = logger;
    }

    protected override Task HandleDomainEventAsync(
        UserAuthenticationFailedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Authentication failed. Reason={ReasonCode} IpHash={IpHash} IdentityHash={IdentityHash} OutboxId={OutboxId}",
            domainEvent.ReasonCode,
            domainEvent.IpHash,
            domainEvent.IdentityHash,
            outboxMessageId);
        return Task.CompletedTask;
    }
}
