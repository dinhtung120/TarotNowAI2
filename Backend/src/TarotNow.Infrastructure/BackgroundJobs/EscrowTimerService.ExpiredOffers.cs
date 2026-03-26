using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private async Task ProcessExpiredOffers(IChatFinanceRepository repo, CancellationToken cancellationToken)
    {
        var expired = await repo.GetExpiredOffersAsync(cancellationToken);
        foreach (var item in expired)
        {
            try
            {
                item.Status = QuestionItemStatus.Refunded;
                item.RefundedAt = DateTime.UtcNow;
                await repo.UpdateItemAsync(item, cancellationToken);
                _logger.LogInformation("[EscrowTimer] Expired offer cancelled: {ItemId}", item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Failed to cancel offer: {ItemId}", item.Id);
            }
        }

        if (expired.Count > 0)
        {
            await repo.SaveChangesAsync(cancellationToken);
        }
    }

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

        session.TotalFrozen -= item.AmountDiamond;
        if (session.TotalFrozen < 0)
        {
            session.TotalFrozen = 0;
        }

        await repo.UpdateSessionAsync(session, cancellationToken);
    }
}
