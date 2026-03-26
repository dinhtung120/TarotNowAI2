using TarotNow.Domain.Constants;
using TarotNow.Domain.Entities;

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
        WithdrawalRequest? createdRequest = null;

        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            await _walletRepo.DebitAsync(
                request.UserId,
                "diamond",
                "withdrawal",
                request.AmountDiamond,
                referenceSource: "withdrawal_request",
                description: $"Rút {request.AmountDiamond}💎 (= {plan.NetAmountVnd:N0} VND sau phí 10%)",
                cancellationToken: transactionCt);

            await ValidateDailyLimitAsync(request.UserId, today, transactionCt);
            createdRequest = BuildWithdrawalEntity(request, today, plan);

            await _withdrawalRepo.AddAsync(createdRequest, transactionCt);
            await _withdrawalRepo.SaveChangesAsync(transactionCt);
        }, cancellationToken);

        return createdRequest!;
    }

    private static WithdrawalRequest BuildWithdrawalEntity(
        CreateWithdrawalCommand request,
        DateOnly today,
        WithdrawalPlan plan)
    {
        return new WithdrawalRequest
        {
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
