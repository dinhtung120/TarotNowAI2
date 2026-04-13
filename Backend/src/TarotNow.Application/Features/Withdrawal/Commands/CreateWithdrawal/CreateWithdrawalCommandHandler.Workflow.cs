using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.Constants;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;

public partial class CreateWithdrawalCommandHandler
{
    /// <summary>
    /// Tính kế hoạch quy đổi rút tiền từ diamond sang VND.
    /// Luồng xử lý: quy đổi amountVnd theo tỉ lệ cấu hình, tính phí 10% và net amount thực nhận.
    /// </summary>
    private static WithdrawalPlan BuildWithdrawalPlan(long amountDiamond)
    {
        var amountVnd = amountDiamond * EconomyConstants.VndPerDiamond;
        var feeVnd = (long)Math.Ceiling(amountVnd * 0.10);
        var netAmountVnd = amountVnd - feeVnd;
        return new WithdrawalPlan(amountVnd, feeVnd, netAmountVnd);
    }

    /// <summary>
    /// Thực thi workflow tạo withdrawal request trong transaction.
    /// Luồng xử lý: kiểm tra idempotency, debit ví, kiểm tra lại daily-limit trong transaction, tạo request và lưu dữ liệu.
    /// </summary>
    private async Task<WithdrawalRequest> ExecuteWithdrawalAsync(
        CreateWithdrawalCommand request,
        DateOnly today,
        WithdrawalPlan plan,
        CancellationToken cancellationToken)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(request.IdempotencyKey);
        var withdrawalRequestId = BuildDeterministicWithdrawalRequestId(request.UserId, normalizedIdempotencyKey);

        var existingRequest = await _withdrawalRepo.GetByIdAsync(withdrawalRequestId, cancellationToken);
        if (existingRequest is not null)
        {
            // Idempotent retry: request đã tồn tại thì trả lại luôn, không debit lại ví.
            return existingRequest;
        }

        WithdrawalRequest? createdRequest = null;

        await _transactionCoordinator.ExecuteAsync(
            async transactionCt =>
            {
                var requestInTransaction = await _withdrawalRepo.GetByIdAsync(withdrawalRequestId, transactionCt);
                if (requestInTransaction is not null)
                {
                    // Nhánh chống race-condition khi request được tạo bởi giao dịch song song.
                    createdRequest = requestInTransaction;
                    return;
                }

                await _walletRepo.DebitAsync(
                    request.UserId,
                    CurrencyType.Diamond,
                    "withdrawal",
                    request.AmountDiamond,
                    referenceSource: "withdrawal_request",
                    referenceId: withdrawalRequestId.ToString(),
                    description: $"Rút {request.AmountDiamond}💎 (= {plan.NetAmountVnd:N0} VND sau phí 10%)",
                    idempotencyKey: $"withdrawal_{normalizedIdempotencyKey}",
                    cancellationToken: transactionCt);
                await _domainEventPublisher.PublishAsync(
                    new MoneyChangedDomainEvent
                    {
                        UserId = request.UserId,
                        Currency = CurrencyType.Diamond,
                        ChangeType = TransactionType.Withdrawal,
                        DeltaAmount = -request.AmountDiamond,
                        ReferenceId = withdrawalRequestId.ToString()
                    },
                    transactionCt);
                // Trừ ví trước khi tạo request để đảm bảo số dư luôn phản ánh trạng thái giữ tiền cho giao dịch rút.

                await ValidateDailyLimitAsync(request.UserId, today, transactionCt);
                // Re-check limit trong transaction để chặn đua song song tạo nhiều request cùng ngày.

                createdRequest = BuildWithdrawalEntity(request, today, plan, withdrawalRequestId);

                await _withdrawalRepo.AddAsync(createdRequest, transactionCt);
                await _withdrawalRepo.SaveChangesAsync(transactionCt);
                // Lưu request trong cùng transaction với debit để đảm bảo nhất quán tài chính.
            },
            cancellationToken);

        return createdRequest!;
    }

    /// <summary>
    /// Chuẩn hóa idempotency key đầu vào.
    /// Luồng xử lý: trim dữ liệu, kiểm tra bắt buộc và giới hạn độ dài tối đa 128 ký tự.
    /// </summary>
    private static string NormalizeIdempotencyKey(string? idempotencyKey)
    {
        var normalized = idempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            // Idempotency key bắt buộc để chống tạo request trùng.
            throw new ArgumentException("IdempotencyKey là bắt buộc.", nameof(idempotencyKey));
        }

        if (normalized.Length > 128)
        {
            // Khóa quá dài dễ gây lỗi lưu trữ/logging nên bị chặn sớm.
            throw new ArgumentException("IdempotencyKey tối đa 128 ký tự.", nameof(idempotencyKey));
        }

        return normalized;
    }

    /// <summary>
    /// Dựng request id deterministic theo user + idempotency key.
    /// Luồng xử lý: băm SHA-256 chuỗi đầu vào và lấy 16 byte đầu để tạo Guid ổn định.
    /// </summary>
    private static Guid BuildDeterministicWithdrawalRequestId(Guid userId, string normalizedIdempotencyKey)
    {
        var input = $"{userId:N}:{normalizedIdempotencyKey}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return new Guid(hash.AsSpan(0, 16));
    }

    /// <summary>
    /// Dựng entity WithdrawalRequest từ command và kế hoạch quy đổi.
    /// Luồng xử lý: map dữ liệu ngân hàng, số tiền quy đổi và trạng thái khởi tạo pending.
    /// </summary>
    private static WithdrawalRequest BuildWithdrawalEntity(
        CreateWithdrawalCommand request,
        DateOnly today,
        WithdrawalPlan plan,
        Guid requestId)
    {
        return new WithdrawalRequest
        {
            Id = requestId,
            UserId = request.UserId,
            BusinessDateUtc = today,
            AmountDiamond = request.AmountDiamond,
            AmountVnd = plan.AmountVnd,
            FeeVnd = plan.FeeVnd,
            NetAmountVnd = plan.NetAmountVnd,
            BankName = request.BankName,
            BankAccountName = request.BankAccountName,
            BankAccountNumber = request.BankAccountNumber,
            Status = "pending"
        };
    }
}
