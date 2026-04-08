using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    /// <summary>
    /// Quét và xử lý danh sách offer đã hết hạn.
    /// Luồng xử lý: lấy expired item từ repository, xử lý từng candidate và log thành công/thất bại.
    /// </summary>
    private async Task ProcessExpiredOffers(
        RefundDependencies dependencies,
        CancellationToken cancellationToken)
    {
        var expired = await dependencies.FinanceRepository.GetExpiredOffersAsync(cancellationToken);
        foreach (var item in expired)
        {
            try
            {
                await ProcessExpiredOfferCandidateAsync(
                    dependencies,
                    item.Id,
                    cancellationToken);

                _logger.LogInformation("[EscrowTimer] Expired offer cancelled: {ItemId}", item.Id);
                // Log vận hành để theo dõi số lượng offer hết hạn được xử lý mỗi vòng.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Failed to cancel offer: {ItemId}", item.Id);
                // Không dừng vòng quét khi một offer lỗi để tránh backlog tăng nhanh.
            }
        }
    }

    /// <summary>
    /// Xử lý một candidate expired offer và cập nhật conversation tương ứng.
    /// Luồng xử lý: chạy transaction refund, sau đó mark conversation expired khi có hoàn tiền thực tế.
    /// </summary>
    private async Task ProcessExpiredOfferCandidateAsync(
        RefundDependencies dependencies,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        var outcome = await ExecuteExpiredOfferRefundAsync(dependencies, candidateId, cancellationToken);
        if (string.IsNullOrWhiteSpace(outcome.ConversationId) == false && outcome.RefundedAmount > 0)
        {
            await MarkConversationExpiredAsync(
                dependencies,
                outcome.ConversationId,
                $"Reader không phản hồi trong thời gian quy định. Đã hoàn {outcome.RefundedAmount} 💎.",
                cancellationToken);
            // Đồng bộ trạng thái conversation để tránh giữ hội thoại ở trạng thái chờ không hợp lệ.
        }
    }
}
