using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Đồng bộ projection trạng thái Reader theo domain event presence.
/// Rule nghiệp vụ:
/// - online chỉ nâng offline -> online.
/// - nếu reader đang busy thì online event không override.
/// - offline luôn hạ về offline để phản ánh mất kết nối thực tế.
/// </summary>
public sealed class UserStatusChangedReaderProjectionDomainEventHandler
    : IdempotentDomainEventNotificationHandler<UserStatusChangedDomainEvent>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    public UserStatusChangedReaderProjectionDomainEventHandler(
        IReaderProfileRepository readerProfileRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    protected override async Task HandleDomainEventAsync(
        UserStatusChangedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        if (!ReaderOnlineStatus.TryNormalize(domainEvent.Status, out var normalizedStatus))
        {
            return;
        }

        var profile = await _readerProfileRepository.GetByUserIdAsync(domainEvent.UserId, cancellationToken);
        if (profile is null)
        {
            return;
        }

        var currentStatus = ReaderOnlineStatus.NormalizeOrDefault(profile.Status);
        if (normalizedStatus == ReaderOnlineStatus.Online && currentStatus == ReaderOnlineStatus.Busy)
        {
            return;
        }

        if (string.Equals(currentStatus, normalizedStatus, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        profile.Status = normalizedStatus;
        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);
    }
}
