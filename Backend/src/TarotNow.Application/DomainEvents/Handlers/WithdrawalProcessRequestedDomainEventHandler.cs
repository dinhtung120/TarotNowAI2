using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Xử lý domain event admin duyệt/từ chối yêu cầu rút tiền.
/// </summary>
public sealed partial class WithdrawalProcessRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<WithdrawalProcessRequestedDomainEvent>
{
    private const string WithdrawalReferenceSource = "withdrawal_request";

    private readonly IWithdrawalRepository _withdrawalRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler xử lý withdrawal request.
    /// </summary>
    public WithdrawalProcessRequestedDomainEventHandler(
        IWithdrawalRepository withdrawalRepository,
        IUserRepository userRepository,
        IWalletRepository walletRepository,
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _withdrawalRepository = withdrawalRepository;
        _userRepository = userRepository;
        _walletRepository = walletRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        WithdrawalProcessRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var action = NormalizeAction(domainEvent.Action);
        var processKey = NormalizeProcessIdempotencyKey(domainEvent.IdempotencyKey);
        await EnsureProcessKeyNotUsedByAnotherRequestAsync(processKey, domainEvent.RequestId, cancellationToken);

        var request = await LoadPendingRequestForProcessingAsync(domainEvent.RequestId, processKey, cancellationToken);
        if (request == null)
        {
            return;
        }

        await EnsureAdminCanProcessAsync(domainEvent.AdminId, cancellationToken);
        var processedAtUtc = DateTime.UtcNow;
        await ApplyActionAsync(request, domainEvent, action, cancellationToken);

        request.AdminId = domainEvent.AdminId;
        request.ProcessedAt = processedAtUtc;
        request.ProcessIdempotencyKey = processKey;
        request.UpdatedAt = processedAtUtc;

        await _withdrawalRepository.UpdateAsync(request, cancellationToken);
        await _withdrawalRepository.SaveChangesAsync(cancellationToken);
        await PublishProcessedEventAsync(request, domainEvent, action, processedAtUtc, cancellationToken);
    }

    private async Task EnsureProcessKeyNotUsedByAnotherRequestAsync(
        string processIdempotencyKey,
        Guid requestId,
        CancellationToken cancellationToken)
    {
        var existingByProcessKey = await _withdrawalRepository.GetByProcessIdempotencyKeyAsync(
            processIdempotencyKey,
            cancellationToken);
        if (existingByProcessKey == null || existingByProcessKey.Id == requestId)
        {
            return;
        }

        throw new BadRequestException("Idempotency key da duoc su dung cho yeu cau khac.");
    }

    private async Task<WithdrawalRequest?> LoadPendingRequestForProcessingAsync(
        Guid requestId,
        string processIdempotencyKey,
        CancellationToken cancellationToken)
    {
        var request = await _withdrawalRepository.GetByIdForUpdateAsync(requestId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy yêu cầu rút tiền.");

        if (string.Equals(request.ProcessIdempotencyKey, processIdempotencyKey, StringComparison.Ordinal))
        {
            return null;
        }

        if (string.Equals(request.Status, WithdrawalRequestStatus.Pending, StringComparison.OrdinalIgnoreCase))
        {
            return request;
        }

        throw new BadRequestException($"Yêu cầu đang ở trạng thái '{request.Status}', không thể xử lý.");
    }

    private async Task EnsureAdminCanProcessAsync(Guid adminId, CancellationToken cancellationToken)
    {
        var admin = await _userRepository.GetByIdAsync(adminId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy admin.");
        if (string.Equals(admin.Role, UserRole.Admin, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        throw new BadRequestException("Chỉ admin mới có quyền duyệt/từ chối yêu cầu rút tiền.");
    }

    private async Task ApplyActionAsync(
        WithdrawalRequest request,
        WithdrawalProcessRequestedDomainEvent domainEvent,
        string action,
        CancellationToken cancellationToken)
    {
        if (action == WithdrawalProcessAction.Approve)
        {
            request.AdminNote = NormalizeOptionalNote(domainEvent.AdminNote);
            request.Status = WithdrawalRequestStatus.Approved;
            await ConsumeFrozenDiamondAsync(request, cancellationToken);
            await PublishApproveMoneyChangedAsync(request, cancellationToken);
            return;
        }

        request.AdminNote = NormalizeRejectReason(domainEvent.AdminNote);
        request.Status = WithdrawalRequestStatus.Rejected;
        await RefundFrozenDiamondAsync(request, cancellationToken);
        await PublishRejectMoneyChangedAsync(request, cancellationToken);
    }

}
