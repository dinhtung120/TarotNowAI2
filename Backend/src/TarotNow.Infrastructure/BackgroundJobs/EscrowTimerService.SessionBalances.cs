using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    /// <summary>
    /// Cập nhật lại total frozen của finance session sau khi một item được refund/release.
    /// Luồng xử lý: lock session theo item, bỏ qua nếu không tồn tại, trừ amount và chặn âm.
    /// </summary>
    private static async Task UpdateSessionFrozenBalanceAsync(
        IChatFinanceRepository repo,
        ChatQuestionItem item,
        CancellationToken cancellationToken)
    {
        var session = await repo.GetSessionForUpdateAsync(item.FinanceSessionId, cancellationToken);
        if (session == null)
        {
            // Edge case: session không tồn tại ở thời điểm cập nhật, bỏ qua an toàn.
            return;
        }

        session.TotalFrozen = Math.Max(0, session.TotalFrozen - item.AmountDiamond);
        // Chặn âm để đảm bảo số dư frozen không bị lệch do race condition hoặc dữ liệu biên.
        await repo.UpdateSessionAsync(session, cancellationToken);
    }
}
