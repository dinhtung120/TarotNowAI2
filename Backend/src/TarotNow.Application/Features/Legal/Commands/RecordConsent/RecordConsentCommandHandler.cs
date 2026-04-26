using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Legal.Commands.RecordConsent;

// Handler ghi nhận consent pháp lý theo nguyên tắc idempotent.
public class RecordConsentCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RecordConsentCommandHandlerRequestedDomainEvent>
{
    private readonly IUserConsentRepository _consentRepository;

    /// <summary>
    /// Khởi tạo handler để xử lý ghi nhận consent.
    /// Luồng xử lý: nhận repository để kiểm tra consent hiện có và lưu consent mới khi cần.
    /// </summary>
    public RecordConsentCommandHandlerRequestedDomainEventHandler(
        IUserConsentRepository consentRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _consentRepository = consentRepository;
    }

    /// <summary>
    /// Ghi nhận consent cho một tài liệu pháp lý.
    /// Luồng xử lý: kiểm tra consent đã tồn tại chưa để tránh trùng lặp, sau đó tạo bản ghi mới khi chưa có.
    /// </summary>
    public async Task<bool> Handle(RecordConsentCommand request, CancellationToken cancellationToken)
    {
        var newConsent = new UserConsent(
            request.UserId,
            request.DocumentType,
            request.Version,
            request.IpAddress,
            request.UserAgent);
        await _consentRepository.TryAddAsync(newConsent, cancellationToken);

        return true;
    }

    protected override async Task HandleDomainEventAsync(
        RecordConsentCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
