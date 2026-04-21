using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler xử lý cập nhật hồ sơ Reader.
/// </summary>
public sealed class ReaderProfileUpdateRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReaderProfileUpdateRequestedDomainEvent>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    /// <summary>
    /// Khởi tạo handler cập nhật hồ sơ Reader.
    /// </summary>
    public ReaderProfileUpdateRequestedDomainEventHandler(
        IReaderProfileRepository readerProfileRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReaderProfileUpdateRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var profile = await _readerProfileRepository.GetByUserIdAsync(domainEvent.UserId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader. Bạn cần được admin duyệt trước.");

        ReaderProfileUpdateDomainRules.ApplyBioPatch(profile, domainEvent);
        ReaderProfileUpdateDomainRules.ApplyPricePatch(profile, domainEvent);
        ReaderProfileUpdateDomainRules.ApplySpecialtiesPatch(profile, domainEvent);
        ReaderProfileUpdateDomainRules.ApplyYearsOfExperiencePatch(profile, domainEvent);
        ReaderProfileUpdateDomainRules.ApplySocialLinksPatch(profile, domainEvent);
        ReaderProfileUpdateDomainRules.EnsureProfileInvariants(profile);

        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);
        domainEvent.Updated = true;
    }
}
