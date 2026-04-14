using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler side-effects khi logout/revoke sessions.
/// </summary>
public sealed class UserLoggedOutDomainEventHandler
    : IdempotentDomainEventNotificationHandler<UserLoggedOutDomainEvent>
{
    private static readonly ActivitySource ActivitySource = new("TarotNow.Auth");
    private static readonly TimeSpan AccessBlacklistTtl = TimeSpan.FromMinutes(20);
    private static readonly TimeSpan SessionRevokedTtl = TimeSpan.FromMinutes(30);
    private readonly ICacheService _cacheService;
    private readonly ILogger<UserLoggedOutDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo logout event handler.
    /// </summary>
    public UserLoggedOutDomainEventHandler(
        ICacheService cacheService,
        ILogger<UserLoggedOutDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        UserLoggedOutDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("auth.logout");
        activity?.SetTag("auth.user_id", domainEvent.UserId);
        activity?.SetTag("auth.revoke_all", domainEvent.RevokeAll);
        activity?.SetTag("auth.session_id", domainEvent.SessionId);

        if (domainEvent.RevokeAll)
        {
            var userSessions = await _cacheService.GetAsync<List<Guid>>(
                AuthEventCacheHelpers.BuildUserSessionIndexKey(domainEvent.UserId),
                cancellationToken) ?? new List<Guid>();
            foreach (var userSessionId in userSessions)
            {
                await MarkSessionRevokedAndBlacklistAsync(userSessionId, cancellationToken);
            }

            await _cacheService.RemoveAsync(AuthEventCacheHelpers.BuildUserSessionIndexKey(domainEvent.UserId), cancellationToken);
        }

        if (domainEvent.SessionId is Guid sessionId && sessionId != Guid.Empty)
        {
            await MarkSessionRevokedAndBlacklistAsync(sessionId, cancellationToken);
            await AuthEventCacheHelpers.RemoveSessionFromUserIndexAsync(_cacheService, domainEvent.UserId, sessionId, cancellationToken);
        }

        _logger.LogInformation(
            "Auth logout processed. UserId={UserId} SessionId={SessionId} RevokeAll={RevokeAll} Reason={Reason} OutboxId={OutboxId}",
            domainEvent.UserId,
            domainEvent.SessionId,
            domainEvent.RevokeAll,
            domainEvent.Reason,
            outboxMessageId);
    }

    private async Task MarkSessionRevokedAndBlacklistAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        var snapshot = await _cacheService.GetAsync<AuthSessionCacheSnapshot>(
            AuthEventCacheHelpers.BuildSessionKey(sessionId),
            cancellationToken);
        if (snapshot is not null)
        {
            await AuthEventCacheHelpers.BlacklistAccessJtiAsync(
                _cacheService,
                snapshot.LastAccessJti,
                AccessBlacklistTtl,
                cancellationToken);
        }

        await AuthEventCacheHelpers.MarkSessionRevokedAsync(
            _cacheService,
            sessionId,
            SessionRevokedTtl,
            cancellationToken);
        await _cacheService.RemoveAsync(AuthEventCacheHelpers.BuildSessionKey(sessionId), cancellationToken);
    }
}
