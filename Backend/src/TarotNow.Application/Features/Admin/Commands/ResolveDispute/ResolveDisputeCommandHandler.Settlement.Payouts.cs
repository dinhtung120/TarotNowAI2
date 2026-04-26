using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandExecutor
{
    /// <summary>
    /// Thực hiện nhánh release: giải ngân cho reader và thu phí nền tảng.
    /// Luồng xử lý: tính fee/net, release net cho reader, consume fee nếu có, cập nhật trạng thái item.
    /// </summary>
    private async Task ReleaseToReaderAsync(
        Guid adminId,
        ChatQuestionItem item,
        string auditMetadata,
        CancellationToken cancellationToken)
    {
        // Rule phí: thu 10% làm tròn lên để đảm bảo phí nền tảng không bị hụt do số lẻ.
        var fee = (long)Math.Ceiling(item.AmountDiamond * 0.10);
        var readerAmount = item.AmountDiamond - fee;

        await _walletRepo.ReleaseAsync(
            item.PayerId,
            item.ReceiverId,
            readerAmount,
            referenceSource: "admin_dispute_resolve",
            referenceId: item.Id.ToString(),
            description: $"Admin {adminId} resolve: release {readerAmount}💎",
            metadataJson: auditMetadata,
            idempotencyKey: $"settle_release_{item.Id}",
            cancellationToken: cancellationToken);
        await PublishMoneyChangedAsync(
            item.ReceiverId,
            readerAmount,
            TransactionType.EscrowRelease,
            item.Id,
            cancellationToken);
        await PublishMoneyChangedAsync(
            item.PayerId,
            0,
            TransactionType.EscrowRelease,
            item.Id,
            cancellationToken);

        if (fee > 0)
        {
            // Chỉ thu phí khi fee dương để tránh bút toán consume 0 vô nghĩa.
            await _walletRepo.ConsumeAsync(
                item.PayerId,
                fee,
                referenceSource: "platform_fee",
                referenceId: item.Id.ToString(),
                description: $"Admin {adminId} settle fee {fee}💎",
                metadataJson: auditMetadata,
                idempotencyKey: $"settle_fee_{item.Id}",
                cancellationToken: cancellationToken);
        }

        // Đổi state item sau khi bút toán release/fee hoàn tất.
        item.Status = QuestionItemStatus.Released;
        item.ReleasedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Thực hiện nhánh refund: hoàn toàn bộ số tiền về người dùng trả phí.
    /// Luồng xử lý: gọi refund với metadata audit và cập nhật trạng thái item thành Refunded.
    /// </summary>
    private async Task RefundToUserAsync(
        Guid adminId,
        ChatQuestionItem item,
        string auditMetadata,
        CancellationToken cancellationToken)
    {
        await _walletRepo.RefundAsync(
            item.PayerId,
            item.AmountDiamond,
            referenceSource: "admin_dispute_resolve",
            referenceId: item.Id.ToString(),
            description: $"Admin {adminId} resolve: refund {item.AmountDiamond}💎",
            metadataJson: auditMetadata,
            idempotencyKey: $"settle_refund_{item.Id}",
            cancellationToken: cancellationToken);
        await PublishMoneyChangedAsync(
            item.PayerId,
            item.AmountDiamond,
            TransactionType.EscrowRefund,
            item.Id,
            cancellationToken);

        // Đổi state item sau khi hoàn tiền thành công.
        item.Status = QuestionItemStatus.Refunded;
        item.RefundedAt = DateTime.UtcNow;
    }
}
