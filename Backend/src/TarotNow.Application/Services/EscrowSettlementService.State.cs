using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using System.Globalization;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Services;

// Phần helper trạng thái cho settlement escrow để gom các thao tác cập nhật/emit event dùng chung.
public sealed partial class EscrowSettlementService
{
    /// <summary>
    /// Tạo mô tả giao dịch release/fee để thống nhất log ví và dễ đối soát vận hành.
    /// Luồng xử lý: phân nhánh theo isAutoRelease rồi trả cặp mô tả cho bút toán release và platform fee.
    /// </summary>
    private static (string ReleaseDescription, string FeeDescription) BuildDescriptions(
        bool isAutoRelease,
        long readerAmount,
        long fee,
        decimal feeRate)
    {
        var feePercentText = (feeRate * 100m).ToString("0.##", CultureInfo.InvariantCulture);
        if (isAutoRelease)
        {
            // Nhánh auto-release dùng wording riêng để phân biệt với thao tác xác nhận thủ công.
            return ($"Auto-release {readerAmount}💎 (fee {fee}💎)", $"Platform fee auto {feePercentText}% = {fee}💎");
        }

        // Nhánh manual release giữ mô tả nghiệp vụ rõ ràng cho luồng xác nhận bởi người dùng.
        return ($"Release {readerAmount}💎 (fee {fee}💎) cho reader", $"Platform fee {feePercentText}% = {fee}💎");
    }

    /// <summary>
    /// Tạo mô tả giao dịch release/fee cho nhánh giải ngân gộp theo session.
    /// Luồng xử lý: trả mô tả rõ là session payout để tiện truy vết đối soát.
    /// </summary>
    private static (string ReleaseDescription, string FeeDescription) BuildSessionDescriptions(
        bool isAutoRelease,
        long readerAmount,
        long fee,
        decimal feeRate)
    {
        var feePercentText = (feeRate * 100m).ToString("0.##", CultureInfo.InvariantCulture);
        if (isAutoRelease)
        {
            return ($"Auto-release session {readerAmount}💎 (fee {fee}💎)", $"Platform fee auto session {feePercentText}% = {fee}💎");
        }

        return ($"Release session {readerAmount}💎 (fee {fee}💎) cho reader", $"Platform fee session {feePercentText}% = {fee}💎");
    }

    /// <summary>
    /// Áp trạng thái Released cho item để đóng escrow và mở cửa sổ khiếu nại hậu giao dịch.
    /// Luồng xử lý: ghi các mốc thời gian release/dispute và chỉ set ConfirmedAt cho luồng không tự động.
    /// </summary>
    private static void ApplyReleasedState(ChatQuestionItem item, bool isAutoRelease, int disputeWindowHours)
    {
        ApplyReleasedState(item, isAutoRelease, disputeWindowHours, DateTime.UtcNow);
    }

    /// <summary>
    /// Áp trạng thái Released cho item tại một mốc thời gian cố định.
    /// Luồng xử lý: dùng chung timestamp để đồng bộ nhiều item trong cùng đợt settlement.
    /// </summary>
    private static void ApplyReleasedState(
        ChatQuestionItem item,
        bool isAutoRelease,
        int disputeWindowHours,
        DateTime settledAtUtc)
    {
        var normalizedDisputeWindowHours = disputeWindowHours > 0 ? disputeWindowHours : 24;
        item.Status = QuestionItemStatus.Released;
        item.ReleasedAt = settledAtUtc;
        item.DisputeWindowStart = settledAtUtc;
        item.DisputeWindowEnd = settledAtUtc.AddHours(normalizedDisputeWindowHours);
        item.AutoReleaseAt = null;
        // Đồng bộ state item sang Released và mở dispute window theo policy cấu hình.

        if (isAutoRelease == false)
        {
            item.ConfirmedAt = settledAtUtc;
            // Luồng xác nhận thủ công cần lưu mốc ConfirmedAt để truy vết hành động xác nhận.
        }
    }

    /// <summary>
    /// Giảm tổng tiền đang đóng băng của finance session sau khi release thành công.
    /// Luồng xử lý: lấy session theo chế độ update, bỏ qua khi không tồn tại, rồi trừ frozen với chặn âm.
    /// </summary>
    private async Task DecreaseSessionFrozenAsync(ChatQuestionItem item, CancellationToken cancellationToken)
    {
        var session = await _financeRepository.GetSessionForUpdateAsync(item.FinanceSessionId, cancellationToken);
        if (session == null)
        {
            // Edge case: session đã bị xóa/không còn tồn tại, bỏ qua để tránh ném lỗi nền.
            return;
        }

        session.TotalFrozen = Math.Max(0, session.TotalFrozen - item.AmountDiamond);
        // Chặn âm để đảm bảo tổng frozen luôn hợp lệ trong trường hợp dữ liệu biên.
        await _financeRepository.UpdateSessionAsync(session, cancellationToken);
    }

    /// <summary>
    /// Phát domain event escrow released để các subscriber xử lý side-effect liên quan.
    /// Luồng xử lý: dựng payload sự kiện từ item đã settle và publish qua domain event publisher.
    /// </summary>
    private Task PublishReleasedEventAsync(
        ChatQuestionItem item,
        long releasedAmountDiamond,
        long feeAmountDiamond,
        bool isAutoRelease,
        CancellationToken cancellationToken)
    {
        return _domainEventPublisher.PublishAsync(new EscrowReleasedDomainEvent
        {
            ItemId = item.Id,
            PayerId = item.PayerId,
            ReceiverId = item.ReceiverId,
            GrossAmountDiamond = item.AmountDiamond,
            ReleasedAmountDiamond = releasedAmountDiamond,
            FeeAmountDiamond = feeAmountDiamond,
            IsAutoRelease = isAutoRelease
        }, cancellationToken);
    }

    /// <summary>
    /// Phát domain event escrow session released để đồng bộ side-effects theo một lần payout.
    /// Luồng xử lý: publish payload tổng hợp gross/released/fee và số item đã release của session.
    /// </summary>
    private Task PublishSessionReleasedEventAsync(
        EscrowSessionReleaseSummary summary,
        CancellationToken cancellationToken)
    {
        return _domainEventPublisher.PublishAsync(new EscrowSessionReleasedDomainEvent
        {
            FinanceSessionId = summary.FinanceSessionId,
            PayerId = summary.PayerId,
            ReceiverId = summary.ReceiverId,
            GrossAmountDiamond = summary.GrossAmountDiamond,
            ReleasedAmountDiamond = summary.ReleasedAmountDiamond,
            FeeAmountDiamond = summary.FeeAmountDiamond,
            ReleasedItemCount = summary.ReleasedItemCount,
            IsAutoRelease = summary.IsAutoRelease
        }, cancellationToken);
    }
}
