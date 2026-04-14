using System.Diagnostics;
using Microsoft.Extensions.Logging;
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
    private static readonly TimeSpan AccessBlacklistTtl = TimeSpan.FromMinutes(20);
    private static readonly TimeSpan SessionRevokedTtl = TimeSpan.FromMinutes(30);
    private readonly ICacheService _cacheService;
    private readonly ILogger<RefreshTokenReplayDetectedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo replay event handler.
    /// </summary>
    public RefreshTokenReplayDetectedDomainEventHandler(
        ICacheService cacheService,
        ILogger<RefreshTokenReplayDetectedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _cacheService = cacheService;
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
            $"auth:security:replay:{domainEvent.SessionId}",
            new
            {
                domainEvent.UserId,
                domainEvent.SessionId,
                domainEvent.FamilyId,
                domainEvent.SourceIpHash,
                domainEvent.OccurredAtUtc
            },
            TimeSpan.FromHours(24),
            cancellationToken);

        var snapshot = await _cacheService.GetAsync<AuthSessionCacheSnapshot>(
            AuthEventCacheHelpers.BuildSessionKey(domainEvent.SessionId),
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
            domainEvent.SessionId,
            SessionRevokedTtl,
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
