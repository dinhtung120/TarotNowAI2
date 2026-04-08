using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    // Kết quả xử lý auto-refund cho một candidate để dùng ở bước cập nhật conversation.
    private readonly record struct AutoRefundOutcome(string? ConversationId, long RefundedAmount);

    /// <summary>
    /// Thực thi transaction auto-refund cho một item candidate.
    /// Luồng xử lý: lock item for update, kiểm tra eligibility, refund ví, cập nhật state/session, publish event và commit.
    /// </summary>
    private async Task<AutoRefundOutcome> ExecuteAutoRefundTransactionAsync(
        RefundDependencies dependencies,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        var outcome = new AutoRefundOutcome(null, 0);

        await dependencies.TransactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await dependencies.FinanceRepository.GetItemForUpdateAsync(candidateId, transactionCt);
            if (item == null || IsEligibleForAutoRefund(item, DateTime.UtcNow) == false)
            {
                // Item không còn tồn tại hoặc không còn hợp lệ để refund thì bỏ qua an toàn.
                return;
            }

            await dependencies.WalletRepository.RefundAsync(
                item.PayerId,
                item.AmountDiamond,
                referenceSource: "chat_question_item",
                referenceId: item.Id.ToString(),
                description: $"Auto-refund {item.AmountDiamond}💎 (reader không reply trong 24h)",
                idempotencyKey: $"settle_refund_{item.Id}",
                cancellationToken: transactionCt);
            // Hoàn lại full amount về payer với idempotency key cố định theo item.

            ApplyRefundState(item, DateTime.UtcNow);
            await dependencies.FinanceRepository.UpdateItemAsync(item, transactionCt);
            await UpdateSessionFrozenBalanceAsync(dependencies.FinanceRepository, item, transactionCt);
            await MarkSessionRefundedWhenFullyReleasedAsync(dependencies.FinanceRepository, item.FinanceSessionId, transactionCt);
            await PublishAutoRefundEventAsync(dependencies, item, transactionCt);
            await dependencies.FinanceRepository.SaveChangesAsync(transactionCt);
            // Đồng bộ state item/session + domain event trong cùng transaction để tránh lệch dữ liệu.

            outcome = new AutoRefundOutcome(item.ConversationRef, item.AmountDiamond);
        }, cancellationToken);

        return outcome;
    }

    /// <summary>
    /// Kiểm tra item có đủ điều kiện auto-refund theo SLA phản hồi hay không.
    /// Luồng xử lý: yêu cầu trạng thái Accepted, chưa reply, có AutoRefundAt và đã tới hạn.
    /// </summary>
    private static bool IsEligibleForAutoRefund(Domain.Entities.ChatQuestionItem item, DateTime now)
    {
        return item.Status == QuestionItemStatus.Accepted
               && item.RepliedAt == null
               && item.AutoRefundAt != null
               && item.AutoRefundAt <= now;
    }

    /// <summary>
    /// Áp trạng thái refunded và dispute window cho item.
    /// Luồng xử lý: set status/mốc refunded và mở cửa sổ khiếu nại 24 giờ.
    /// </summary>
    private static void ApplyRefundState(Domain.Entities.ChatQuestionItem item, DateTime now)
    {
        item.Status = QuestionItemStatus.Refunded;
        item.RefundedAt = now;
        item.DisputeWindowStart = now;
        item.DisputeWindowEnd = now.AddHours(24);
    }

    /// <summary>
    /// Publish domain event EscrowRefunded sau khi hoàn tiền thành công.
    /// Luồng xử lý: dựng payload event từ item và gửi qua domain event publisher.
    /// </summary>
    private static Task PublishAutoRefundEventAsync(
        RefundDependencies dependencies,
        Domain.Entities.ChatQuestionItem item,
        CancellationToken cancellationToken)
    {
        return dependencies.DomainEventPublisher.PublishAsync(new EscrowRefundedDomainEvent
        {
            ItemId = item.Id,
            UserId = item.PayerId,
            AmountDiamond = item.AmountDiamond,
            RefundSource = "reader_sla_timeout"
        }, cancellationToken);
    }
}
