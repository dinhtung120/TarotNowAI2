using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private static async Task UpdateSessionFrozenBalanceAsync(
        IChatFinanceRepository repo,
        ChatQuestionItem item,
        CancellationToken cancellationToken)
    {
        var session = await repo.GetSessionForUpdateAsync(item.FinanceSessionId, cancellationToken);
        if (session == null)
        {
            return;
        }

        session.TotalFrozen = Math.Max(0, session.TotalFrozen - item.AmountDiamond);
        await repo.UpdateSessionAsync(session, cancellationToken);
    }
}
