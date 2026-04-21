using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler side-effects khi phát hiện replay refresh token.
/// </summary>
public sealed class RefreshTokenReplayDetectedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RefreshTokenReplayDetectedDomainEvent>
{
    private static readonly ActivitySource ActivitySource = new("TarotNow.Auth");
    private readonly ICacheService _cacheService;
    private readonly TimeSpan _accessBlacklistTtl;
    private readonly TimeSpan _sessionRevokedTtl;
    private readonly TimeSpan _replayRecordTtl;
    private readonly ILogger<RefreshTokenReplayDetectedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo replay event handler.
    /// </summary>
    public RefreshTokenReplayDetectedDomainEventHandler(
        ICacheService cacheService,
        IAuthSecuritySettings authSecuritySettings,
        ILogger<RefreshTokenReplayDetectedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _cacheService = cacheService;
        _accessBlacklistTtl = TimeSpan.FromSeconds(Math.Max(60, authSecuritySettings.AccessTokenBlacklistTtlSeconds));
        _sessionRevokedTtl = TimeSpan.FromSeconds(Math.Max(60, authSecuritySettings.SessionRevocationTtlSeconds));
        _replayRecordTtl = TimeSpan.FromSeconds(Math.Max(300, authSecuritySettings.ReplaySecurityRecordTtlSeconds));
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        RefreshTokenReplayDetectedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("auth.replay_detected");
        activity?.SetTag("auth.user_id", domainEvent.UserId);
        activity?.SetTag("auth.session_id", domainEvent.SessionId);
        activity?.SetTag("auth.family_id", domainEvent.FamilyId);

        await _cacheService.SetAsync(
            AuthCacheKeys.BuildReplaySecurityKey(domainEvent.SessionId),
            new
            {
                domainEvent.UserId,
                domainEvent.SessionId,
                domainEvent.FamilyId,
                domainEvent.SourceIpHash,
                domainEvent.OccurredAtUtc
            },
            _replayRecordTtl,
            cancellationToken);

        if (domainEvent.SessionId == Guid.Empty)
        {
            _logger.LogWarning(
                "Refresh token replay detected without session id. UserId={UserId} FamilyId={FamilyId} OutboxId={OutboxId}",
                domainEvent.UserId,
                domainEvent.FamilyId,
                outboxMessageId);
            return;
        }

        var snapshot = await _cacheService.GetAsync<AuthSessionCacheSnapshot>(
            AuthEventCacheHelpers.BuildSessionKey(domainEvent.SessionId),
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
            domainEvent.SessionId,
            _sessionRevokedTtl,
            cancellationToken);
        await _cacheService.RemoveAsync(AuthEventCacheHelpers.BuildSessionKey(domainEvent.SessionId), cancellationToken);

        _logger.LogWarning(
            "Refresh token replay detected. UserId={UserId} SessionId={SessionId} FamilyId={FamilyId} SourceIpHash={SourceIpHash} OutboxId={OutboxId}",
            domainEvent.UserId,
            domainEvent.SessionId,
            domainEvent.FamilyId,
            domainEvent.SourceIpHash,
            outboxMessageId);
    }
}
