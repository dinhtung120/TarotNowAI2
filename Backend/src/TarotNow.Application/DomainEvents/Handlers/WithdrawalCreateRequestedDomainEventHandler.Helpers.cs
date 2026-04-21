using System.Security.Cryptography;
using System.Text;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class WithdrawalCreateRequestedDomainEventHandler
{
    private const string ErrorReaderOnly = "Chỉ tài khoản Reader mới được rút tiền.";
    private const string ErrorInsufficientBalance = "Số dư Diamond không đủ để tạo yêu cầu rút tiền.";
    private const string ErrorMissingPayoutInfo = "Bạn chưa cập nhật thông tin ngân hàng nhận tiền. Vui lòng vào Profile để bổ sung trước khi rút.";
    private const string ErrorIdempotencyRequired = "Idempotency key là bắt buộc.";
    private const string ErrorMinimumWithdrawTemplate = "Số lượng rút tối thiểu là {0} Diamond.";
    private const string ErrorIdempotencyMaxLengthTemplate = "Idempotency key tối đa {0} ký tự.";

    private static WithdrawalRequest BuildPendingRequest(
        WithdrawalCreateRequestedDomainEvent domainEvent,
        User user,
        Guid requestId,
        string normalizedRequestKey,
        DateOnly businessWeekStartUtc,
        WithdrawalPlan plan)
    {
        return new WithdrawalRequest
        {
            Id = requestId,
            UserId = domainEvent.UserId,
            BusinessWeekStartUtc = businessWeekStartUtc,
            AmountDiamond = domainEvent.AmountDiamond,
            AmountVnd = plan.AmountVnd,
            FeeVnd = plan.FeeVnd,
            NetAmountVnd = plan.NetAmountVnd,
            BankName = user.PayoutBankName!.Trim(),
            BankBin = user.PayoutBankBin!.Trim(),
            BankAccountName = user.PayoutBankAccountHolder!.Trim(),
            BankAccountNumber = user.PayoutBankAccountNumber!.Trim(),
            UserNote = NormalizeOptionalNote(domainEvent.UserNote),
            RequestIdempotencyKey = normalizedRequestKey,
            Status = WithdrawalRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    private static void ValidateUserCanWithdraw(User user, long amountDiamond)
    {
        if (!string.Equals(user.Role, UserRole.TarotReader, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException(ErrorReaderOnly);
        }

        if (amountDiamond < WithdrawalPolicyConstants.MinimumWithdrawDiamond)
        {
            throw new BadRequestException(
                string.Format(ErrorMinimumWithdrawTemplate, WithdrawalPolicyConstants.MinimumWithdrawDiamond));
        }

        if (user.Wallet.DiamondBalance < amountDiamond)
        {
            throw new BadRequestException(ErrorInsufficientBalance);
        }

        if (string.IsNullOrWhiteSpace(user.PayoutBankName)
            || string.IsNullOrWhiteSpace(user.PayoutBankBin)
            || string.IsNullOrWhiteSpace(user.PayoutBankAccountNumber)
            || string.IsNullOrWhiteSpace(user.PayoutBankAccountHolder))
        {
            throw new BadRequestException(ErrorMissingPayoutInfo);
        }
    }

    private static void HydrateCreateResult(
        WithdrawalCreateRequestedDomainEvent domainEvent,
        WithdrawalRequest request)
    {
        domainEvent.RequestId = request.Id;
        domainEvent.Status = request.Status;
    }

    private static string NormalizeIdempotencyKey(string? idempotencyKey)
    {
        var normalized = idempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BadRequestException(ErrorIdempotencyRequired);
        }

        if (normalized.Length > WithdrawalPolicyConstants.IdempotencyKeyMaxLength)
        {
            throw new BadRequestException(
                string.Format(ErrorIdempotencyMaxLengthTemplate, WithdrawalPolicyConstants.IdempotencyKeyMaxLength));
        }

        return normalized;
    }

    private static string? NormalizeOptionalNote(string? note)
    {
        if (string.IsNullOrWhiteSpace(note))
        {
            return null;
        }

        var trimmed = note.Trim();
        return trimmed.Length > WithdrawalPolicyConstants.NoteMaxLength
            ? trimmed[..WithdrawalPolicyConstants.NoteMaxLength]
            : trimmed;
    }

    private static Guid BuildDeterministicRequestId(Guid userId, string normalizedIdempotencyKey)
    {
        var source = $"{userId:N}:{normalizedIdempotencyKey}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(source));
        return new Guid(hash.AsSpan(0, 16));
    }

    private static string BuildFreezeIdempotencyKey(Guid requestId)
        => $"withdrawal_freeze_{requestId}";

    private static DateOnly ResolveBusinessWeekStartUtc(DateTime nowUtc)
    {
        var currentDate = DateOnly.FromDateTime(nowUtc);
        var offset = ((int)currentDate.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        return currentDate.AddDays(-offset);
    }

    private static WithdrawalPlan BuildWithdrawalPlan(long amountDiamond)
    {
        var amountVnd = amountDiamond * EconomyConstants.VndPerDiamond;
        var feeVnd = (long)Math.Ceiling(amountVnd * (double)WithdrawalPolicyConstants.FeeRate);
        var netAmountVnd = amountVnd - feeVnd;
        return new WithdrawalPlan(amountVnd, feeVnd, netAmountVnd);
    }

    private readonly record struct WithdrawalPlan(long AmountVnd, long FeeVnd, long NetAmountVnd);
}
