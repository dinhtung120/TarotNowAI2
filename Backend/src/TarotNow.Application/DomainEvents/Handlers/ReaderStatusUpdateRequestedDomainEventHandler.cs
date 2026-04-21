using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler xử lý cập nhật trạng thái Reader.
/// </summary>
public sealed class ReaderStatusUpdateRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReaderStatusUpdateRequestedDomainEvent>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    /// <summary>
    /// Khởi tạo handler cập nhật trạng thái Reader.
    /// </summary>
    public ReaderStatusUpdateRequestedDomainEventHandler(
        IReaderProfileRepository readerProfileRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReaderStatusUpdateRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        if (!ReaderOnlineStatus.TryNormalize(domainEvent.Status, out var normalizedStatus))
        {
            throw new BadRequestException($"Trạng thái '{domainEvent.Status}' không hợp lệ. Chỉ chấp nhận: offline, busy.");
        }

        if (normalizedStatus == ReaderOnlineStatus.Online)
        {
            throw new BadRequestException("Trạng thái 'online' được cập nhật tự động khi kết nối. Truyền 'busy' hoặc 'offline' để đổi trạng thái thủ công.");
        }

        var profile = await _readerProfileRepository.GetByUserIdAsync(domainEvent.UserId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader.");

        profile.Status = normalizedStatus;
        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);
        domainEvent.Updated = true;
    }
}
