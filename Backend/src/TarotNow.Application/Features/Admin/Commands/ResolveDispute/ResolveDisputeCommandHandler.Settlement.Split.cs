using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler
{
    // Kết quả tính chia tiền gồm gross cho reader, phần refund user, fee nền tảng và net reader thực nhận.
    private readonly record struct SplitAmounts(long ReaderGross, long RefundAmount, long Fee, long ReaderNet);
    // Ngữ cảnh dùng chung cho các bước chuyển tiền trong nhánh split.
    private readonly record struct SplitTransferContext(
        Guid AdminId,
        ChatQuestionItem Item,
        int SplitPercentToReader,
        string AuditMetadata,
        SplitAmounts Split);

    /// <summary>
    /// Thực thi nhánh split tiền giữa reader và user.
    /// Luồng xử lý: tính các khoản split, thực hiện lần lượt release/fee/refund, cập nhật trạng thái item theo kết quả.
    /// </summary>
    private async Task SplitBetweenReaderAndUserAsync(
        Guid adminId,
        ChatQuestionItem item,
        int splitPercentToReader,
        string auditMetadata,
        CancellationToken cancellationToken)
    {
        var split = CalculateSplitAmounts(item.AmountDiamond, splitPercentToReader);
        var context = new SplitTransferContext(adminId, item, splitPercentToReader, auditMetadata, split);

        // Thực thi từng bút toán theo thứ tự cố định để log/audit dễ đối chiếu.
        await ReleaseSplitToReaderAsync(context, cancellationToken);
        await ConsumeSplitFeeAsync(context, cancellationToken);
        await RefundSplitToUserAsync(context, cancellationToken);

        // Đặt trạng thái item theo kết quả thực tế sau split.
        item.Status = split.ReaderGross > 0 ? QuestionItemStatus.Released : QuestionItemStatus.Refunded;
        item.ReleasedAt = split.ReaderGross > 0 ? DateTime.UtcNow : item.ReleasedAt;
        item.RefundedAt = split.RefundAmount > 0 ? DateTime.UtcNow : item.RefundedAt;
    }

    /// <summary>
    /// Tính các khoản tiền khi xử lý split dispute.
    /// Luồng xử lý: tính gross reader theo phần trăm, phần refund còn lại, fee 10% của gross, rồi tính net reader.
    /// </summary>
    private static SplitAmounts CalculateSplitAmounts(long amountDiamond, int splitPercentToReader)
    {
        // Dùng floor cho reader gross để tránh chi vượt tổng khi làm tròn.
        var readerGross = (long)Math.Floor(amountDiamond * splitPercentToReader / 100.0m);
        var refundAmount = amountDiamond - readerGross;
        // Fee áp trên gross reader, làm tròn lên để nhất quán với nhánh release toàn phần.
        var fee = readerGross <= 0 ? 0 : (long)Math.Ceiling(readerGross * 0.10m);
        // Clamp net về 0 để bảo vệ edge case fee >= gross.
        var readerNet = Math.Max(0, readerGross - fee);
        return new SplitAmounts(readerGross, refundAmount, fee, readerNet);
    }
}
