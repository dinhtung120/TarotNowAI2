using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler side-effects khi login thất bại.
/// </summary>
public sealed class LoginFailedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<LoginFailedDomainEvent>
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<LoginFailedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo login failed event handler.
    /// </summary>
    public LoginFailedDomainEventHandler(
        ICacheService cacheService,
        ILogger<LoginFailedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        LoginFailedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var window = AuthSecurityPolicyConstants.LoginFailureWindow;
        var ipKey = $"auth:login-fail:ip:{domainEvent.IpHash}";
        var identityKey = $"auth:login-fail:identity:{domainEvent.IdentityHash}";

        var ipCount = await _cacheService.IncrementAsync(ipKey, window, cancellationToken);
        var identityCount = await _cacheService.IncrementAsync(identityKey, window, cancellationToken);

        _logger.LogWarning(
            "Login failed. Reason={ReasonCode} IpHash={IpHash} IpCount={IpCount} IdentityHash={IdentityHash} IdentityCount={IdentityCount} OutboxId={OutboxId}",
            domainEvent.ReasonCode,
            domainEvent.IpHash,
            ipCount,
            domainEvent.IdentityHash,
            identityCount,
            outboxMessageId);
    }
}
