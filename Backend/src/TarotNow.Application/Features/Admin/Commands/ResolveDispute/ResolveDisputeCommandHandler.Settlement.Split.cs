using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler
{
    private readonly record struct SplitAmounts(long ReaderGross, long RefundAmount, long Fee, long ReaderNet);
    private readonly record struct SplitTransferContext(
        Guid AdminId,
        ChatQuestionItem Item,
        int SplitPercentToReader,
        string AuditMetadata,
        SplitAmounts Split);

    private async Task SplitBetweenReaderAndUserAsync(
        Guid adminId,
        ChatQuestionItem item,
        int splitPercentToReader,
        string auditMetadata,
        CancellationToken cancellationToken)
    {
        var split = CalculateSplitAmounts(item.AmountDiamond, splitPercentToReader);
        var context = new SplitTransferContext(adminId, item, splitPercentToReader, auditMetadata, split);

        await ReleaseSplitToReaderAsync(context, cancellationToken);
        await ConsumeSplitFeeAsync(context, cancellationToken);
        await RefundSplitToUserAsync(context, cancellationToken);

        item.Status = split.ReaderGross > 0 ? QuestionItemStatus.Released : QuestionItemStatus.Refunded;
        item.ReleasedAt = split.ReaderGross > 0 ? DateTime.UtcNow : item.ReleasedAt;
        item.RefundedAt = split.RefundAmount > 0 ? DateTime.UtcNow : item.RefundedAt;
    }

    private static SplitAmounts CalculateSplitAmounts(long amountDiamond, int splitPercentToReader)
    {
        var readerGross = (long)Math.Floor(amountDiamond * splitPercentToReader / 100.0m);
        var refundAmount = amountDiamond - readerGross;
        var fee = readerGross <= 0 ? 0 : (long)Math.Ceiling(readerGross * 0.10m);
        var readerNet = Math.Max(0, readerGross - fee);
        return new SplitAmounts(readerGross, refundAmount, fee, readerNet);
    }

}
