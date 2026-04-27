using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler đồng bộ projection ReaderProfile (Mongo) từ User write-model (PG).
/// </summary>
public sealed class UserProfileProjectionSyncRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<UserProfileProjectionSyncRequestedDomainEvent>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    public UserProfileProjectionSyncRequestedDomainEventHandler(
        IReaderProfileRepository readerProfileRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        UserProfileProjectionSyncRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var readerProfile = await _readerProfileRepository.GetByUserIdAsync(domainEvent.UserId.ToString(), cancellationToken);
        if (readerProfile is null)
        {
            return;
        }

        if (readerProfile.UpdatedAt.HasValue
            && readerProfile.UpdatedAt.Value >= domainEvent.SourceUpdatedAtUtc)
        {
            // Bỏ qua event cũ hơn hoặc trùng version đã apply trong projection.
            return;
        }

        if (string.Equals(readerProfile.DisplayName, domainEvent.DisplayName, StringComparison.Ordinal)
            && string.Equals(readerProfile.AvatarUrl, domainEvent.AvatarUrl, StringComparison.Ordinal))
        {
            return;
        }

        readerProfile.DisplayName = domainEvent.DisplayName;
        readerProfile.AvatarUrl = domainEvent.AvatarUrl;
        await _readerProfileRepository.UpdateAsync(readerProfile, cancellationToken);
    }
}
