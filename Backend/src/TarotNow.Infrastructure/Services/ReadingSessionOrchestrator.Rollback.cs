using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Services;

public sealed partial class ReadingSessionOrchestrator
{
    // Context rollback gom dữ liệu cần thiết để hoàn tiền khi tạo session thất bại.
    private readonly record struct RollbackContext(
        Guid UserId,
        ReadingSession Session,
        long CostGold,
        long CostDiamond,
        bool GoldDebited,
        bool DiamondDebited,
        Exception StartSessionException);

    /// <summary>
    /// Hoàn lại các khoản đã debit nếu bước tạo session gặp lỗi.
    /// Luồng hoàn tiền theo từng currency đã trừ thực tế và ném lỗi gộp nếu rollback thất bại.
    /// </summary>
    private async Task RollbackDebitsAsync(RollbackContext context)
    {
        try
        {
            if (context.GoldDebited)
            {
                // Hoàn gold trước khi thoát lỗi để giữ cân bằng ví người dùng.
                await CreditRollbackAsync(
                    context.UserId,
                    context.Session.Id,
                    CurrencyType.Gold,
                    context.CostGold,
                    CancellationToken.None);
            }

            if (context.DiamondDebited)
            {
                // Hoàn diamond nếu đã debit, độc lập với nhánh gold.
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
            // Ném lỗi nghiêm trọng khi rollback thất bại vì trạng thái tài chính có thể không nhất quán.
            throw new InvalidOperationException(
                "Lỗi nghiêm trọng: giao dịch không nhất quán khi khởi tạo phiên đọc bài. Vui lòng liên hệ hỗ trợ.",
                new AggregateException(context.StartSessionException, refundException));
        }
    }

    /// <summary>
    /// Tạo giao dịch credit hoàn tiền cho rollback phiên đọc bài.
    /// Luồng dùng idempotency key cố định theo session-currency để tránh hoàn lặp khi retry.
    /// </summary>
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
