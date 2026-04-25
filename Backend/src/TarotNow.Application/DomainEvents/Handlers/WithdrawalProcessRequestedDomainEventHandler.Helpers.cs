using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class WithdrawalProcessRequestedDomainEventHandler
{
    private async Task ConsumeFrozenDiamondAsync(WithdrawalRequest request, CancellationToken cancellationToken)
    {
        await _walletRepository.ConsumeAsync(
            userId: request.UserId,
            amount: request.AmountDiamond,
            referenceSource: WithdrawalReferenceSource,
            referenceId: request.Id.ToString(),
            description: $"Approve withdrawal request {request.Id}",
            idempotencyKey: BuildConsumeIdempotencyKey(request.Id),
            cancellationToken: cancellationToken);
    }

    private async Task RefundFrozenDiamondAsync(WithdrawalRequest request, CancellationToken cancellationToken)
    {
        await _walletRepository.RefundAsync(
            userId: request.UserId,
            amount: request.AmountDiamond,
            referenceSource: WithdrawalReferenceSource,
            referenceId: request.Id.ToString(),
            description: $"Reject withdrawal request {request.Id}",
            idempotencyKey: BuildRefundIdempotencyKey(request.Id),
            cancellationToken: cancellationToken);
    }

    private async Task PublishApproveMoneyChangedAsync(WithdrawalRequest request, CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new WalletSnapshotChangedDomainEvent
            {
                UserId = request.UserId,
                Currency = CurrencyType.Diamond,
                ChangeType = TransactionType.Withdrawal,
                ReferenceId = request.Id.ToString()
            },
            cancellationToken);
    }

    private async Task PublishRejectMoneyChangedAsync(WithdrawalRequest request, CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = request.UserId,
                Currency = CurrencyType.Diamond,
                ChangeType = TransactionType.WithdrawalRefund,
                DeltaAmount = request.AmountDiamond,
                ReferenceId = request.Id.ToString()
            },
            cancellationToken);
    }

    private async Task PublishProcessedEventAsync(
        WithdrawalRequest request,
        WithdrawalProcessRequestedDomainEvent domainEvent,
        string action,
        DateTime processedAtUtc,
        CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new WithdrawalProcessedDomainEvent
            {
                RequestId = request.Id,
                UserId = request.UserId,
                AdminId = domainEvent.AdminId,
                Action = action,
                Status = request.Status,
                AmountDiamond = request.AmountDiamond,
                AdminNote = request.AdminNote,
                ProcessedAtUtc = processedAtUtc
            },
            cancellationToken);
    }

    private static string NormalizeAction(string? action)
    {
        var normalized = action?.Trim().ToLowerInvariant();
        if (normalized is WithdrawalProcessAction.Approve or WithdrawalProcessAction.Reject)
        {
            return normalized;
        }

        throw new BadRequestException("Action phải là 'approve' hoặc 'reject'.");
    }

    private static string NormalizeProcessIdempotencyKey(string? idempotencyKey)
    {
        var normalized = idempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BadRequestException("Idempotency key là bắt buộc.");
        }

        if (normalized.Length > WithdrawalPolicyConstants.IdempotencyKeyMaxLength)
        {
            throw new BadRequestException(
                $"Idempotency key tối đa {WithdrawalPolicyConstants.IdempotencyKeyMaxLength} ký tự.");
        }

        return normalized;
    }

    private static string NormalizeRejectReason(string? reason)
    {
        var normalized = reason?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BadRequestException("Lý do từ chối là bắt buộc.");
        }

        return normalized.Length > WithdrawalPolicyConstants.NoteMaxLength
            ? normalized[..WithdrawalPolicyConstants.NoteMaxLength]
            : normalized;
    }

    private static string? NormalizeOptionalNote(string? note)
    {
        if (string.IsNullOrWhiteSpace(note))
        {
            return null;
        }

        var normalized = note.Trim();
        return normalized.Length > WithdrawalPolicyConstants.NoteMaxLength
            ? normalized[..WithdrawalPolicyConstants.NoteMaxLength]
            : normalized;
    }

    private static string BuildConsumeIdempotencyKey(Guid requestId)
        => $"withdrawal_consume_{requestId}";

    private static string BuildRefundIdempotencyKey(Guid requestId)
        => $"withdrawal_refund_{requestId}";
}
