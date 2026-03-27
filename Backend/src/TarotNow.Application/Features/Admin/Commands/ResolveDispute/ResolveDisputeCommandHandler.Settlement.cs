using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler
{
    private async Task ResolveDisputeAsync(
        ResolveDisputeCommand request,
        string action,
        string auditMetadata,
        CancellationToken cancellationToken)
    {
        var item = await _financeRepo.GetItemForUpdateAsync(request.ItemId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy câu hỏi.");

        if (item.Status != QuestionItemStatus.Disputed)
        {
            throw new BadRequestException("Câu hỏi không ở trạng thái dispute.");
        }

        if (item.ReleasedAt != null || item.RefundedAt != null)
        {
            throw new BadRequestException("Dispute này đã được settle trước đó.");
        }

        if (action == "release")
        {
            await ReleaseToReaderAsync(request.AdminId, item, auditMetadata, cancellationToken);
        }
        else
        {
            await RefundToUserAsync(request.AdminId, item, auditMetadata, cancellationToken);
        }

        await _financeRepo.UpdateItemAsync(item, cancellationToken);
        await ReduceSessionFrozenBalanceAsync(item, cancellationToken);
        await _financeRepo.SaveChangesAsync(cancellationToken);
    }

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

    private async Task ReduceSessionFrozenBalanceAsync(ChatQuestionItem item, CancellationToken cancellationToken)
    {
        var session = await _financeRepo.GetSessionForUpdateAsync(item.FinanceSessionId, cancellationToken);
        if (session == null)
        {
            return;
        }

        session.TotalFrozen -= item.AmountDiamond;
        if (session.TotalFrozen < 0)
        {
            session.TotalFrozen = 0;
        }

        await _financeRepo.UpdateSessionAsync(session, cancellationToken);
    }
}
