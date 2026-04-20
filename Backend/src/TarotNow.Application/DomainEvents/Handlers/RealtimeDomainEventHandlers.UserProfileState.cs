using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler publish realtime reading.quota_changed khi free draw quota thay đổi.
/// </summary>
public sealed class FreeDrawGrantedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<FreeDrawGrantedDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler free draw granted realtime.
    /// </summary>
    public FreeDrawGrantedRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        FreeDrawGrantedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _redisPublisher.PublishAsync(
            RealtimeChannelNames.UserState,
            RealtimeEventNames.ReadingQuotaChanged,
            new
            {
                userId = domainEvent.UserId.ToString(),
                spreadCardCount = domainEvent.SpreadCardCount,
                grantedCount = domainEvent.GrantedCount,
                at = domainEvent.OccurredAtUtc
            },
            cancellationToken);
    }
}

/// <summary>
/// Handler publish realtime title/profile changed khi người dùng được cấp title.
/// </summary>
public sealed class TitleGrantedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<TitleGrantedDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler title granted realtime.
    /// </summary>
    public TitleGrantedRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        TitleGrantedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        await _redisPublisher.PublishAsync(
            RealtimeChannelNames.UserState,
            RealtimeEventNames.TitleChanged,
            new
            {
                userId = domainEvent.UserId.ToString(),
                titleCode = domainEvent.TitleCode,
                source = domainEvent.Source,
                at = domainEvent.OccurredAtUtc
            },
            cancellationToken);

        await _redisPublisher.PublishAsync(
            RealtimeChannelNames.UserState,
            RealtimeEventNames.ProfileChanged,
            new
            {
                userId = domainEvent.UserId.ToString(),
                at = domainEvent.OccurredAtUtc
            },
            cancellationToken);
    }
}

/// <summary>
/// Handler publish realtime UserStatusChanged cho presence.
/// </summary>
public sealed class UserStatusChangedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<UserStatusChangedDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler user status changed realtime.
    /// </summary>
    public UserStatusChangedRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        UserStatusChangedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _redisPublisher.PublishAsync(
            RealtimeChannelNames.UserState,
            RealtimeEventNames.UserStatusChanged,
            new
            {
                userId = domainEvent.UserId,
                status = domainEvent.Status,
                at = domainEvent.OccurredAtUtc
            },
            cancellationToken);
    }
}
