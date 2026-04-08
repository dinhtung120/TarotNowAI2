using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    /// <summary>
    /// Quét và xử lý danh sách item đủ điều kiện auto-refund.
    /// Luồng xử lý: lấy candidate từ repository, xử lý từng item và bắt lỗi cục bộ để không dừng toàn job.
    /// </summary>
    private async Task ProcessAutoRefunds(
        RefundDependencies dependencies,
        CancellationToken cancellationToken)
    {
        var candidates = await dependencies.FinanceRepository.GetItemsForAutoRefundAsync(cancellationToken);
        foreach (var candidate in candidates)
        {
            try
            {
                await ProcessAutoRefundCandidateAsync(
                    dependencies,
                    candidate.Id,
                    cancellationToken);

                _logger.LogInformation(
                    "[EscrowTimer] Auto-refund: {ItemId}, {Amount}💎",
                    candidate.Id,
                    candidate.AmountDiamond);
                // Log thành công để dễ theo dõi sản lượng auto-refund theo vòng quét.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Auto-refund failed: {ItemId}", candidate.Id);
                // Giữ vòng lặp tiếp tục xử lý candidate khác dù một item thất bại.
            }
        }
    }

    /// <summary>
    /// Xử lý một candidate auto-refund và cập nhật trạng thái conversation nếu cần.
    /// Luồng xử lý: chạy transaction refund, sau đó mark conversation expired khi có hoàn tiền thực tế.
    /// </summary>
    private async Task ProcessAutoRefundCandidateAsync(
        RefundDependencies dependencies,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        var outcome = await ExecuteAutoRefundTransactionAsync(dependencies, candidateId, cancellationToken);
        if (string.IsNullOrWhiteSpace(outcome.ConversationId) == false && outcome.RefundedAmount > 0)
        {
            await MarkConversationExpiredAsync(
                dependencies,
                outcome.ConversationId,
                $"Reader không phản hồi trong SLA đã cam kết. Đã hoàn {outcome.RefundedAmount} 💎.",
                cancellationToken);
            // Đánh dấu conversation expired để UI và nghiệp vụ phản ánh đúng kết quả timeout.
        }
    }
}
