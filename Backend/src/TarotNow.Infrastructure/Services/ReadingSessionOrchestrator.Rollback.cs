using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Services;

public sealed partial class ReadingSessionOrchestrator
{
    private readonly record struct RollbackContext(
        Guid UserId,
        ReadingSession Session,
        long CostGold,
        long CostDiamond,
        bool GoldDebited,
        bool DiamondDebited,
        Exception StartSessionException);

    private async Task RollbackDebitsAsync(RollbackContext context)
    {
        try
        {
            if (context.GoldDebited)
            {
                await CreditRollbackAsync(
                    context.UserId,
                    context.Session.Id,
                    CurrencyType.Gold,
                    context.CostGold,
                    CancellationToken.None);
            }

            if (context.DiamondDebited)
            {
                await CreditRollbackAsync(
                    context.UserId,
                    context.Session.Id,
                    CurrencyType.Diamond,
                    context.CostDiamond,
                    CancellationToken.None);
            }
        }
        catch (Exception refundException)
        {
            throw new InvalidOperationException(
                "Lỗi nghiêm trọng: giao dịch không nhất quán khi khởi tạo phiên đọc bài. Vui lòng liên hệ hỗ trợ.",
                new AggregateException(context.StartSessionException, refundException));
        }
    }

    private Task CreditRollbackAsync(
        Guid userId,
        string sessionId,
        string currency,
        long amount,
        CancellationToken cancellationToken)
    {
        return _walletRepository.CreditAsync(
            userId,
            currency,
            TransactionType.ReadingRefund,
            amount,
            referenceSource: "System_Rollback",
            referenceId: sessionId,
            description: $"Hoàn trả {currency} do lỗi hệ thống lưu Session",
            metadataJson: null,
            idempotencyKey: $"refund_rollback_{sessionId}_{currency}",
            cancellationToken: cancellationToken);
    }
}
