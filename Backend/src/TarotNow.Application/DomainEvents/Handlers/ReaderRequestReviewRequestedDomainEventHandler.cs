using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler xử lý duyệt/từ chối đơn Reader.
/// </summary>
public sealed partial class ReaderRequestReviewRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReaderRequestReviewRequestedDomainEvent>
{
    private const string ApproveAction = "approve";
    private const string RejectAction = "reject";

    private readonly IReaderRequestRepository _readerRequestRepository;
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Khởi tạo handler xử lý duyệt đơn Reader.
    /// </summary>
    public ReaderRequestReviewRequestedDomainEventHandler(
        IReaderRequestRepository readerRequestRepository,
        IReaderProfileRepository readerProfileRepository,
        IUserRepository userRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readerRequestRepository = readerRequestRepository;
        _readerProfileRepository = readerProfileRepository;
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReaderRequestReviewRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var readerRequest = await _readerRequestRepository.GetByIdAsync(domainEvent.RequestId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy đơn xin Reader.");

        EnsurePendingRequest(readerRequest);

        var userId = ParseUserId(readerRequest.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");

        if (string.Equals(domainEvent.Action, ApproveAction, StringComparison.Ordinal))
        {
            await ApproveAsync(readerRequest, user, domainEvent, cancellationToken);
            domainEvent.Processed = true;
            return;
        }

        if (string.Equals(domainEvent.Action, RejectAction, StringComparison.Ordinal))
        {
            await RejectAsync(readerRequest, user, domainEvent, cancellationToken);
            domainEvent.Processed = true;
            return;
        }

        throw new BadRequestException("Action không hợp lệ. Chỉ chấp nhận: approve, reject.");
    }

    private async Task ApproveAsync(
        ReaderRequestDto readerRequest,
        User user,
        ReaderRequestReviewRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        EnsureRequestHasMandatoryFieldsForApproval(readerRequest);

        user.ApproveAsReader();
        await _userRepository.UpdateAsync(user, cancellationToken);

        await UpsertReaderProfileFromRequestAsync(readerRequest, user, cancellationToken);

        readerRequest.Status = ReaderApprovalStatus.Approved;
        readerRequest.AdminNote = domainEvent.AdminNote;
        readerRequest.ReviewedBy = domainEvent.AdminId.ToString();
        readerRequest.ReviewedAt = DateTime.UtcNow;
        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);
    }

    private async Task RejectAsync(
        ReaderRequestDto readerRequest,
        User user,
        ReaderRequestReviewRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        user.RejectReaderRequest();
        await _userRepository.UpdateAsync(user, cancellationToken);

        readerRequest.Status = ReaderApprovalStatus.Rejected;
        readerRequest.AdminNote = domainEvent.AdminNote;
        readerRequest.ReviewedBy = domainEvent.AdminId.ToString();
        readerRequest.ReviewedAt = DateTime.UtcNow;
        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);
    }
}
