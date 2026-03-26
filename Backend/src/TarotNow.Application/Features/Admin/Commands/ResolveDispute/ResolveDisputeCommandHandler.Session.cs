using TarotNow.Domain.Entities;

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

        session.TotalFrozen -= item.AmountDiamond;
        if (session.TotalFrozen < 0)
        {
            session.TotalFrozen = 0;
        }

        await _financeRepo.UpdateSessionAsync(session, cancellationToken);
    }
}
