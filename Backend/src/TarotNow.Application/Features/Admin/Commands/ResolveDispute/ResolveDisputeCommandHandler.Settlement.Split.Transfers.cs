using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Chuyển phần tiền net cho reader trong nhánh split.
    /// Luồng xử lý: bỏ qua nếu net <= 0, ngược lại gọi release ví với metadata/idempotency chuẩn.
    /// </summary>
    private async Task ReleaseSplitToReaderAsync(
        SplitTransferContext context,
        CancellationToken cancellationToken)
    {
        if (context.Split.ReaderNet <= 0)
        {
            // Edge case phần reader net bằng 0: không tạo bút toán release.
            return;
        }

        await _walletRepo.ReleaseAsync(
            context.Item.PayerId,
            context.Item.ReceiverId,
            context.Split.ReaderNet,
            referenceSource: "admin_dispute_split",
            referenceId: context.Item.Id.ToString(),
            description: $"Admin {context.AdminId} split release {context.Split.ReaderNet}💎 ({context.SplitPercentToReader}%)",
            metadataJson: context.AuditMetadata,
            idempotencyKey: $"settle_release_{context.Item.Id}",
            cancellationToken: cancellationToken);
        await PublishMoneyChangedAsync(
            context.Item.ReceiverId,
            context.Split.ReaderNet,
            TransactionType.EscrowRelease,
            context.Item.Id,
            cancellationToken);
        await PublishMoneyChangedAsync(
            context.Item.PayerId,
            0,
            TransactionType.EscrowRelease,
            context.Item.Id,
            cancellationToken);
    }

    /// <summary>
    /// Thu phí nền tảng từ phần chia cho reader trong nhánh split.
    /// Luồng xử lý: bỏ qua khi fee <= 0, ngược lại tạo bút toán consume phí.
    /// </summary>
    private async Task ConsumeSplitFeeAsync(
        SplitTransferContext context,
        CancellationToken cancellationToken)
    {
        if (context.Split.Fee <= 0)
        {
            // Edge case fee bằng 0: không phát sinh bút toán consume.
            return;
        }

        await _walletRepo.ConsumeAsync(
            context.Item.PayerId,
            context.Split.Fee,
            referenceSource: "platform_fee",
            referenceId: context.Item.Id.ToString(),
            description: $"Admin {context.AdminId} split fee {context.Split.Fee}💎",
            metadataJson: context.AuditMetadata,
            idempotencyKey: $"settle_fee_{context.Item.Id}",
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Hoàn phần còn lại về user trong nhánh split.
    /// Luồng xử lý: bỏ qua khi refund amount <= 0, ngược lại tạo bút toán refund.
    /// </summary>
    private async Task RefundSplitToUserAsync(
        SplitTransferContext context,
        CancellationToken cancellationToken)
    {
        if (context.Split.RefundAmount <= 0)
        {
            // Edge case không còn phần refund: bỏ qua bước hoàn tiền.
            return;
        }

        await _walletRepo.RefundAsync(
            context.Item.PayerId,
            context.Split.RefundAmount,
            referenceSource: "admin_dispute_split",
            referenceId: context.Item.Id.ToString(),
            description: $"Admin {context.AdminId} split refund {context.Split.RefundAmount}💎",
            metadataJson: context.AuditMetadata,
            idempotencyKey: $"settle_refund_{context.Item.Id}",
            cancellationToken: cancellationToken);
        await PublishMoneyChangedAsync(
            context.Item.PayerId,
            context.Split.RefundAmount,
            TransactionType.EscrowRefund,
            context.Item.Id,
            cancellationToken);
    }
}
