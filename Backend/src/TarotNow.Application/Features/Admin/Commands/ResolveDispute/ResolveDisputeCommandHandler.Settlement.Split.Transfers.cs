using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler
{
    private async Task ReleaseSplitToReaderAsync(
        SplitTransferContext context,
        CancellationToken cancellationToken)
    {
        if (context.Split.ReaderNet <= 0)
        {
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
    }

    private async Task ConsumeSplitFeeAsync(
        SplitTransferContext context,
        CancellationToken cancellationToken)
    {
        if (context.Split.Fee <= 0)
        {
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

    private async Task RefundSplitToUserAsync(
        SplitTransferContext context,
        CancellationToken cancellationToken)
    {
        if (context.Split.RefundAmount <= 0)
        {
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
    }
}
