using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler
{
    private async Task ReduceSessionFrozenBalanceAsync(ChatQuestionItem item, CancellationToken cancellationToken)
    {
        var session = await _financeRepo.GetSessionForUpdateAsync(item.FinanceSessionId, cancellationToken);
        if (session == null)
        {
            return;
        }

        session.TotalFrozen = Math.Max(0, session.TotalFrozen - item.AmountDiamond);
        await _financeRepo.UpdateSessionAsync(session, cancellationToken);
    }

    private async Task FreezeReaderIfDisputeThresholdExceededAsync(
        ChatQuestionItem item,
        CancellationToken cancellationToken)
    {
        var fromUtc = DateTime.UtcNow.AddDays(-7);
        var recentDisputes = await _financeRepo.CountRecentDisputesByReceiverAsync(item.ReceiverId, fromUtc, cancellationToken);
        if (recentDisputes <= 3)
        {
            return;
        }

        var profile = await _readerProfileRepository.GetByUserIdAsync(item.ReceiverId.ToString(), cancellationToken);
        if (profile == null)
        {
            return;
        }

        profile.Status = ReaderOnlineStatus.Offline;
        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);
    }
}
