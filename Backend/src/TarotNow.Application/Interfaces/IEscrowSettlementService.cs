using System.Collections.Generic;
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

    /// <summary>
    /// Áp dụng giải ngân gộp theo toàn bộ accepted item trong một finance session.
    /// Luồng xử lý: tính tổng gross/fee/released theo session, ghi ledger một lần, cập nhật trạng thái item/session và trả summary.
    /// </summary>
    Task<EscrowSessionReleaseSummary?> ApplySessionReleaseAsync(
        ChatFinanceSession session,
        IReadOnlyCollection<ChatQuestionItem> items,
        bool isAutoRelease,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Kết quả giải ngân gộp theo finance session.
/// </summary>
public sealed class EscrowSessionReleaseSummary
{
    public Guid FinanceSessionId { get; init; }

    public Guid PayerId { get; init; }

    public Guid ReceiverId { get; init; }

    public long GrossAmountDiamond { get; init; }

    public long FeeAmountDiamond { get; init; }

    public long ReleasedAmountDiamond { get; init; }

    public int ReleasedItemCount { get; init; }

    public bool IsAutoRelease { get; init; }
}
