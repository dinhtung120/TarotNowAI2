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
    private readonly ICacheService _cacheService;
    private readonly TimeSpan _accessBlacklistTtl;
    private readonly TimeSpan _sessionRevokedTtl;
    private readonly ILogger<UserLoggedOutDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo logout event handler.
    /// </summary>
    public UserLoggedOutDomainEventHandler(
        ICacheService cacheService,
        IAuthSecuritySettings authSecuritySettings,
        ILogger<UserLoggedOutDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _cacheService = cacheService;
        _accessBlacklistTtl = TimeSpan.FromSeconds(Math.Max(60, authSecuritySettings.AccessTokenBlacklistTtlSeconds));
        _sessionRevokedTtl = TimeSpan.FromSeconds(Math.Max(60, authSecuritySettings.SessionRevocationTtlSeconds));
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
            var cachedSessionMembers = await _cacheService.GetSetMembersAsync(
                AuthEventCacheHelpers.BuildUserSessionIndexKey(domainEvent.UserId),
                cancellationToken);
            var sessionIdsFromCache = cachedSessionMembers
                         .Select(static value => Guid.TryParse(value, out var parsed) ? parsed : Guid.Empty)
                         .Where(static sessionId => sessionId != Guid.Empty);
            var sessionIds = domainEvent.SessionIds
                .Concat(sessionIdsFromCache)
                .Distinct();
            foreach (var userSessionId in sessionIds)
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
                _accessBlacklistTtl,
                cancellationToken);
        }

        await AuthEventCacheHelpers.MarkSessionRevokedAsync(
            _cacheService,
            sessionId,
            _sessionRevokedTtl,
            cancellationToken);
        await _cacheService.RemoveAsync(AuthEventCacheHelpers.BuildSessionKey(sessionId), cancellationToken);
    }
}
