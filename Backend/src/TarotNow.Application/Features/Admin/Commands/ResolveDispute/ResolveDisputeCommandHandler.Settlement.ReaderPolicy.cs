using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Giảm tổng frozen balance của phiên tài chính sau khi một item đã được settle.
    /// Luồng xử lý: tải session để update, trừ amount item có clamp về 0, lưu session.
    /// </summary>
    private async Task ReduceSessionFrozenBalanceAsync(ChatQuestionItem item, CancellationToken cancellationToken)
    {
        var session = await _financeRepo.GetSessionForUpdateAsync(item.FinanceSessionId, cancellationToken);
        if (session == null)
        {
            // Edge case phiên đã bị xóa/không tồn tại: bỏ qua để không ném lỗi sai ngữ cảnh.
            return;
        }

        // Clamp về 0 để tránh total frozen âm khi dữ liệu thực tế lệch do xử lý trước đó.
        session.TotalFrozen = Math.Max(0, session.TotalFrozen - item.AmountDiamond);
        await _financeRepo.UpdateSessionAsync(session, cancellationToken);
    }

    /// <summary>
    /// Đóng băng trạng thái online reader khi vượt ngưỡng tranh chấp gần đây.
    /// Luồng xử lý: đếm dispute 7 ngày, nếu vượt ngưỡng thì chuyển profile reader sang offline.
    /// </summary>
    private async Task FreezeReaderIfDisputeThresholdExceededAsync(
        ChatQuestionItem item,
        CancellationToken cancellationToken)
    {
        var lookbackDays = _systemConfigSettings.AdminDisputeReaderFreezeLookbackDays;
        var threshold = _systemConfigSettings.AdminDisputeReaderFreezeThreshold;

        var fromUtc = DateTime.UtcNow.AddDays(-lookbackDays);
        var recentDisputes = await _financeRepo.CountRecentDisputesByReceiverAsync(item.ReceiverId, fromUtc, cancellationToken);
        if (recentDisputes <= threshold)
        {
            // Chỉ can thiệp khi số dispute vượt ngưỡng cấu hình trong cửa sổ lookback.
            return;
        }

        var profile = await _readerProfileRepository.GetByUserIdAsync(item.ReceiverId.ToString(), cancellationToken);
        if (profile == null)
        {
            // Edge case thiếu profile reader: bỏ qua thay đổi trạng thái để tránh null reference.
            return;
        }

        // Đổi reader về offline để hạn chế phát sinh dispute mới trong giai đoạn cần kiểm soát.
        profile.Status = ReaderOnlineStatus.Offline;
        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);
    }
}
