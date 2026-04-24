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
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo handler cập nhật hồ sơ Reader.
    /// </summary>
    public ReaderProfileUpdateRequestedDomainEventHandler(
        IReaderProfileRepository readerProfileRepository,
        ISystemConfigSettings systemConfigSettings,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readerProfileRepository = readerProfileRepository;
        _systemConfigSettings = systemConfigSettings;
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
        ReaderProfileUpdateDomainRules.ApplyPricePatch(
            profile,
            domainEvent,
            _systemConfigSettings.ReaderMinDiamondPerQuestion);
        ReaderProfileUpdateDomainRules.ApplySpecialtiesPatch(profile, domainEvent);
        ReaderProfileUpdateDomainRules.ApplyYearsOfExperiencePatch(
            profile,
            domainEvent,
            _systemConfigSettings.ReaderMinYearsOfExperience);
        ReaderProfileUpdateDomainRules.ApplySocialLinksPatch(profile, domainEvent);
        ReaderProfileUpdateDomainRules.EnsureProfileInvariants(
            profile,
            _systemConfigSettings.ReaderMinYearsOfExperience,
            _systemConfigSettings.ReaderMinDiamondPerQuestion);

        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);
        domainEvent.Updated = true;
    }
}
