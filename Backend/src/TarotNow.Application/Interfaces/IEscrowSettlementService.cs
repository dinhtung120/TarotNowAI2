using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

// Contract xử lý settlement escrow để chốt giải ngân/hoàn tiền theo kết quả câu hỏi.
public interface IEscrowSettlementService
{
    /// <summary>
    /// Áp dụng giải ngân cho một item escrow khi đủ điều kiện chốt thanh toán.
    /// Luồng xử lý: nhận item cần xử lý, phân nhánh auto/manual theo isAutoRelease và cập nhật sổ cái liên quan.
    /// </summary>
    Task ApplyReleaseAsync(
        ChatQuestionItem item,
        bool isAutoRelease,
        CancellationToken cancellationToken = default);
}
