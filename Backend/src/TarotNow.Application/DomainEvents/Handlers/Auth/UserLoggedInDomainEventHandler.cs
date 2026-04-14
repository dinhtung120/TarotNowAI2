using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler side-effects khi user login thành công.
/// </summary>
public sealed class UserLoggedInDomainEventHandler
    : IdempotentDomainEventNotificationHandler<UserLoggedInDomainEvent>
{
    private static readonly ActivitySource ActivitySource = new("TarotNow.Auth");
    private readonly ICacheService _cacheService;
    private readonly ILogger<UserLoggedInDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo login event handler.
    /// </summary>
    public UserLoggedInDomainEventHandler(
        ICacheService cacheService,
        ILogger<UserLoggedInDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        UserLoggedInDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("auth.login");
        activity?.SetTag("auth.user_id", domainEvent.UserId);
        activity?.SetTag("auth.session_id", domainEvent.SessionId);
        activity?.SetTag("auth.device_id", domainEvent.DeviceId);

        await _cacheService.SetAsync(
            AuthEventCacheHelpers.BuildSessionKey(domainEvent.SessionId),
            BuildSessionSnapshot(domainEvent),
            TimeSpan.FromDays(30),
            cancellationToken);
        await AuthEventCacheHelpers.AddSessionToUserIndexAsync(_cacheService, domainEvent.UserId, domainEvent.SessionId, cancellationToken);

        _logger.LogInformation(
            "Auth login success. UserId={UserId} SessionId={SessionId} DeviceId={DeviceId} OutboxId={OutboxId}",
            domainEvent.UserId,
            domainEvent.SessionId,
            domainEvent.DeviceId,
            outboxMessageId);
    }

    private static AuthSessionCacheSnapshot BuildSessionSnapshot(UserLoggedInDomainEvent ev)
    {
        return new AuthSessionCacheSnapshot
        {
            UserId = ev.UserId,
            SessionId = ev.SessionId,
            DeviceId = ev.DeviceId,
            UserAgentHash = ev.UserAgentHash,
            LastIpHash = ev.IpHash,
            LastSeenAtUtc = ev.OccurredAtUtc,
            LastAccessJti = ev.AccessTokenJti
        };
    }
}
