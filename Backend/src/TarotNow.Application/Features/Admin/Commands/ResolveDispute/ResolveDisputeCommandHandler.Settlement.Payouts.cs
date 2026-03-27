using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler
{
    private async Task ReleaseToReaderAsync(
        Guid adminId,
        ChatQuestionItem item,
        string auditMetadata,
        CancellationToken cancellationToken)
    {
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

        if (fee > 0)
        {
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

        item.Status = QuestionItemStatus.Released;
        item.ReleasedAt = DateTime.UtcNow;
    }

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

        item.Status = QuestionItemStatus.Refunded;
        item.RefundedAt = DateTime.UtcNow;
    }

}
