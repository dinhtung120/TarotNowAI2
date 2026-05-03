using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler hậu xử lý khi hệ thống cấp free draw từ inventory.
/// </summary>
public sealed class FreeDrawGrantedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<FreeDrawGrantedDomainEvent>
{
    private readonly IFreeDrawCreditRepository _freeDrawCreditRepository;

    /// <summary>
    /// Khởi tạo handler FreeDrawGrantedDomainEvent.
    /// </summary>
    public FreeDrawGrantedDomainEventHandler(
        IFreeDrawCreditRepository freeDrawCreditRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _freeDrawCreditRepository = freeDrawCreditRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        FreeDrawGrantedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        await _freeDrawCreditRepository.AddCreditsAsync(
            domainEvent.UserId,
            domainEvent.SpreadCardCount,
            domainEvent.GrantedCount,
            cancellationToken);
    }
}
