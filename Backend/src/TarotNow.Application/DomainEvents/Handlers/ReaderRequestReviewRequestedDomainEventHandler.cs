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
    private readonly INotificationRepository _notificationRepository;
    private readonly IRedisPublisher _redisPublisher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuthSessionRepository _authSessionRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler xử lý duyệt đơn Reader.
    /// </summary>
    public ReaderRequestReviewRequestedDomainEventHandler(
        IReaderRequestRepository readerRequestRepository,
        IReaderProfileRepository readerProfileRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        IRedisPublisher redisPublisher,
        IRefreshTokenRepository refreshTokenRepository,
        IAuthSessionRepository authSessionRepository,
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readerRequestRepository = readerRequestRepository;
        _readerProfileRepository = readerProfileRepository;
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _redisPublisher = redisPublisher;
        _refreshTokenRepository = refreshTokenRepository;
        _authSessionRepository = authSessionRepository;
        _domainEventPublisher = domainEventPublisher;
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

        var reviewedAtUtc = DateTime.UtcNow;
        readerRequest.Status = ReaderApprovalStatus.Approved;
        readerRequest.AdminNote = domainEvent.AdminNote;
        readerRequest.ReviewedBy = domainEvent.AdminId.ToString();
        readerRequest.ReviewedAt = reviewedAtUtc;
        AppendReviewHistory(readerRequest, domainEvent, ReaderApprovalStatus.Approved, reviewedAtUtc);
        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);

        await RevokeSessionsAfterRoleChangeAsync(user.Id, cancellationToken);
        await PublishReviewNotificationAsync(readerRequest, domainEvent, ReaderApprovalStatus.Approved, cancellationToken);
    }

    private async Task RejectAsync(
        ReaderRequestDto readerRequest,
        User user,
        ReaderRequestReviewRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        user.RejectReaderRequest();
        await _userRepository.UpdateAsync(user, cancellationToken);

        var reviewedAtUtc = DateTime.UtcNow;
        readerRequest.Status = ReaderApprovalStatus.Rejected;
        readerRequest.AdminNote = domainEvent.AdminNote;
        readerRequest.ReviewedBy = domainEvent.AdminId.ToString();
        readerRequest.ReviewedAt = reviewedAtUtc;
        AppendReviewHistory(readerRequest, domainEvent, ReaderApprovalStatus.Rejected, reviewedAtUtc);
        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);

        await PublishReviewNotificationAsync(readerRequest, domainEvent, ReaderApprovalStatus.Rejected, cancellationToken);
    }
}
