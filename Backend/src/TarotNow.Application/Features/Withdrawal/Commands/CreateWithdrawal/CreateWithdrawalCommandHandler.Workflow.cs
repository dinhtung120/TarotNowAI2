using TarotNow.Application.Common.Constants;
using TarotNow.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;

public partial class CreateWithdrawalCommandHandler
{
    private static WithdrawalPlan BuildWithdrawalPlan(long amountDiamond)
    {
        var amountVnd = amountDiamond * EconomyConstants.VndPerDiamond;
        var feeVnd = (long)Math.Ceiling(amountVnd * 0.10);
        var netAmountVnd = amountVnd - feeVnd;
        return new WithdrawalPlan(amountVnd, feeVnd, netAmountVnd);
    }

    private async Task<WithdrawalRequest> ExecuteWithdrawalAsync(
        CreateWithdrawalCommand request,
        DateOnly today,
        WithdrawalPlan plan,
        CancellationToken cancellationToken)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(request.IdempotencyKey);
        var withdrawalRequestId = BuildDeterministicWithdrawalRequestId(request.UserId, normalizedIdempotencyKey);
        var existingRequest = await _withdrawalRepo.GetByIdAsync(withdrawalRequestId, cancellationToken);
        if (existingRequest != null)
        {
            return existingRequest;
        }

        WithdrawalRequest? createdRequest = null;

        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var requestInTransaction = await _withdrawalRepo.GetByIdAsync(withdrawalRequestId, transactionCt);
            if (requestInTransaction != null)
            {
                createdRequest = requestInTransaction;
                return;
            }

            await _walletRepo.DebitAsync(
                request.UserId,
                "diamond",
                "withdrawal",
                request.AmountDiamond,
                referenceSource: "withdrawal_request",
                referenceId: withdrawalRequestId.ToString(),
                description: $"Rút {request.AmountDiamond}💎 (= {plan.NetAmountVnd:N0} VND sau phí 10%)",
                idempotencyKey: $"withdrawal_{normalizedIdempotencyKey}",
                cancellationToken: transactionCt);

            await ValidateDailyLimitAsync(request.UserId, today, transactionCt);
            createdRequest = BuildWithdrawalEntity(request, today, plan, withdrawalRequestId);

            await _withdrawalRepo.AddAsync(createdRequest, transactionCt);
            await _withdrawalRepo.SaveChangesAsync(transactionCt);
        }, cancellationToken);

        return createdRequest!;
    }

    private static string NormalizeIdempotencyKey(string? idempotencyKey)
    {
        var normalized = idempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new ArgumentException("IdempotencyKey là bắt buộc.", nameof(idempotencyKey));
        }

        if (normalized.Length > 128)
        {
            throw new ArgumentException("IdempotencyKey tối đa 128 ký tự.", nameof(idempotencyKey));
        }

        return normalized;
    }

    private static Guid BuildDeterministicWithdrawalRequestId(Guid userId, string normalizedIdempotencyKey)
    {
        var input = $"{userId:N}:{normalizedIdempotencyKey}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return new Guid(hash.AsSpan(0, 16));
    }

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
