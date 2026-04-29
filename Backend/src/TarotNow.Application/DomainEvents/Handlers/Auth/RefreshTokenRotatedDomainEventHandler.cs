using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler side-effects khi refresh token rotation thành công.
/// </summary>
public sealed class RefreshTokenRotatedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RefreshTokenRotatedDomainEvent>
{
    private static readonly ActivitySource ActivitySource = new("TarotNow.Auth");
    private readonly ICacheService _cacheService;
    private readonly TimeSpan _sessionCacheTtl;
    private readonly ILogger<RefreshTokenRotatedDomainEventHandler> _logger;

    public RefreshTokenRotatedDomainEventHandler(
        ICacheService cacheService,
        IAuthSecuritySettings authSecuritySettings,
        ILogger<RefreshTokenRotatedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _cacheService = cacheService;
        _sessionCacheTtl = TimeSpan.FromSeconds(Math.Max(60, authSecuritySettings.SessionCacheTtlSeconds));
        _logger = logger;
    }

    protected override async Task HandleDomainEventAsync(
        RefreshTokenRotatedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("auth.refresh");
        activity?.SetTag("auth.user_id", domainEvent.UserId);
        activity?.SetTag("auth.session_id", domainEvent.SessionId);
        activity?.SetTag("auth.new_token_id", domainEvent.NewTokenId);

        var key = AuthEventCacheHelpers.BuildSessionKey(domainEvent.SessionId);
        var snapshot = await _cacheService.GetAsync<AuthSessionCacheSnapshot>(key, cancellationToken)
            ?? new AuthSessionCacheSnapshot
            {
                UserId = domainEvent.UserId,
                SessionId = domainEvent.SessionId,
                DeviceId = domainEvent.DeviceId
            };

        snapshot.LastSeenAtUtc = domainEvent.OccurredAtUtc;
        snapshot.LastAccessJti = domainEvent.AccessTokenJti;
        if (!string.IsNullOrWhiteSpace(domainEvent.IpHash))
        {
            snapshot.LastIpHash = domainEvent.IpHash;
        }

        if (!string.IsNullOrWhiteSpace(domainEvent.UserAgentHash))
        {
            snapshot.UserAgentHash = domainEvent.UserAgentHash;
        }

        await _cacheService.SetAsync(key, snapshot, _sessionCacheTtl, cancellationToken);
        await AuthEventCacheHelpers.AddSessionToUserIndexAsync(
            _cacheService,
            domainEvent.UserId,
            domainEvent.SessionId,
            _sessionCacheTtl,
            cancellationToken);

        _logger.LogInformation(
            "Auth refresh success. UserId={UserId} SessionId={SessionId} OldTokenId={OldTokenId} NewTokenId={NewTokenId} Jti={Jti} OutboxId={OutboxId}",
            domainEvent.UserId,
            domainEvent.SessionId,
            domainEvent.OldTokenId,
            domainEvent.NewTokenId,
            domainEvent.AccessTokenJti,
            outboxMessageId);
    }
}
